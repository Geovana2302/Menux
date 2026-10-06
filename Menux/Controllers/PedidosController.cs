using Menux.Data;
using Menux.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Menux.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PedidosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Pedidos?statusPreparo=Recebido&mesaId=1 (Filtro da Cozinha)
        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] string? statusPreparo, [FromQuery] int? mesaId)
        {
            var query = _context.PedidosItens
                .Include(p => p.Mesa)
                .Include(p => p.ItemCardapio)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(statusPreparo))
                query = query.Where(p => p.StatusPreparo.ToLower() == statusPreparo.ToLower());

            if (mesaId.HasValue)
                query = query.Where(p => p.MesaId == mesaId.Value);

            var pedidos = await query.ToListAsync();
            return Ok(pedidos); // 200 OK
        }

        // GET: api/Pedidos/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var pedido = await _context.PedidosItens
                .Include(p => p.Mesa)
                .Include(p => p.ItemCardapio)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
                return NotFound(new { mensagem = "Pedido não encontrado." }); // 404 Not Found

            return Ok(pedido); // 200 OK
        }

        // POST: api/Pedidos
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] PedidoItem novoPedido)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // 400 Bad Request

            // Regra 1: Validar se a mesa existe e está 'Ocupada'
            var mesa = await _context.Mesas.FindAsync(novoPedido.MesaId);
            if (mesa == null)
                return NotFound(new { mensagem = "Mesa informada não existe." }); // 404 Not Found

            if (mesa.Status != "Ocupada")
                return BadRequest(new { mensagem = "Não é possível lançar pedidos para mesas que não estejam com status 'Ocupada'." }); // 400 Bad Request

            // Regra 2: Validar se o item existe e está disponível no cardápio
            var item = await _context.ItensCardapio.FindAsync(novoPedido.ItemCardapioId);
            if (item == null)
                return NotFound(new { mensagem = "Item do cardápio não existe." }); // 404 Not Found

            if (!item.Disponivel)
                return BadRequest(new { mensagem = "Este item está indisponível no cardápio no momento." }); // 400 Bad Request

            // Regra 3: Congelar preço unitário
            novoPedido.PrecoUnitario = item.Preco;
            novoPedido.DataHoraSolicitacao = DateTime.Now;
            novoPedido.StatusPreparo = "Recebido";

            _context.PedidosItens.Add(novoPedido);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(BuscarPorId), new { id = novoPedido.Id }, novoPedido); // 201 Created
        }

        // PUT: api/Pedidos/{id}/status (Atualização do ciclo de preparo)
        [HttpPut("{id}/status")]
        public async Task<IActionResult> AtualizarStatus(int id, [FromBody] string novoStatus)
        {
            var pedido = await _context.PedidosItens.FindAsync(id);
            if (pedido == null)
                return NotFound(new { mensagem = "Pedido não encontrado." }); // 404 Not Found

            var statusValidos = new[] { "Recebido", "Em Preparo", "Pronto", "Entregue" };
            if (!statusValidos.Contains(novoStatus))
                return BadRequest(new { mensagem = "Status inválido." }); // 400 Bad Request

            pedido.StatusPreparo = novoStatus;
            await _context.SaveChangesAsync();

            return Ok(pedido); // 200 OK
        }

        // DELETE: api/Pedidos/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelarPedido(int id)
        {
            var pedido = await _context.PedidosItens.FindAsync(id);
            if (pedido == null)
                return NotFound(new { mensagem = "Pedido não encontrado." }); // 404 Not Found

            // Regra 4: Restrição de cancelamento se já estiver no preparo ou pronto
            if (pedido.StatusPreparo == "Em Preparo" || pedido.StatusPreparo == "Pronto")
                return BadRequest(new { mensagem = "Não é permitido cancelar pedidos que já estão em preparo ou prontos pela cozinha." }); // 400 Bad Request

            _context.PedidosItens.Remove(pedido);
            await _context.SaveChangesAsync();

            return NoContent(); // 204 No Content
        }
    }
}