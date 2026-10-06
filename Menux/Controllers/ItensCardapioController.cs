using Menux.Data;
using Menux.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Menux.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItensCardapioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ItensCardapioController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ItensCardapio?categoria=Bebida&disponivel=true (Filtro)
        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] string? categoria, [FromQuery] bool? disponivel)
        {
            var query = _context.ItensCardapio.AsQueryable();

            if (!string.IsNullOrWhiteSpace(categoria))
                query = query.Where(i => i.Categoria.ToLower() == categoria.ToLower());

            if (disponivel.HasValue)
                query = query.Where(i => i.Disponivel == disponivel.Value);

            var itens = await query.ToListAsync();
            return Ok(itens); // 200 OK
        }

        // GET: api/ItensCardapio/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var item = await _context.ItensCardapio.FindAsync(id);
            if (item == null)
                return NotFound(new { mensagem = "Item do cardápio não encontrado." }); // 404 Not Found

            return Ok(item); // 200 OK
        }

        // POST: api/ItensCardapio
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] ItemCardapio novoItem)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // 400 Bad Request

            _context.ItensCardapio.Add(novoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(BuscarPorId), new { id = novoItem.Id }, novoItem); // 201 Created
        }

        // PUT: api/ItensCardapio/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] ItemCardapio itemAtualizado)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // 400 Bad Request

            var itemExistente = await _context.ItensCardapio.FindAsync(id);
            if (itemExistente == null)
                return NotFound(new { mensagem = "Item não localizado para atualização." }); // 404 Not Found

            itemExistente.Nome = itemAtualizado.Nome;
            itemExistente.Descricao = itemAtualizado.Descricao;
            itemExistente.Preco = itemAtualizado.Preco;
            itemExistente.Categoria = itemAtualizado.Categoria;
            itemExistente.Disponivel = itemAtualizado.Disponivel;

            await _context.SaveChangesAsync();
            return Ok(itemExistente); // 200 OK
        }

        // DELETE: api/ItensCardapio/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var item = await _context.ItensCardapio.FindAsync(id);
            if (item == null)
                return NotFound(new { mensagem = "Item não encontrado." }); // 404 Not Found

            bool possuiPedidos = await _context.PedidosItens.AnyAsync(p => p.ItemCardapioId == id);
            if (possuiPedidos)
                return BadRequest(new { mensagem = "Não é permitido excluir um item que já possui histórico de pedidos." }); // 400 Bad Request

            _context.ItensCardapio.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent(); // 204 No Content
        }
    }
}