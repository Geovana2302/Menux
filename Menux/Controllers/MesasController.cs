using Menux.Data;
using Menux.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Menux.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MesasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MesasController(AppDbContext context)
        {
            _context = context;
        }

        // Endpoint para listar registros (200 OK)
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var mesas = await _context.Mesas.ToListAsync();
            return Ok(mesas);
        }

        // Endpoint para consultar por ID (200 OK ou 404 Not Found)
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var mesa = await _context.Mesas.FindAsync(id);

            if (mesa == null)
                return NotFound();

            return Ok(mesa);
        }

        // Endpoint para cadastrar uma nova mesa
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Mesa mesa)
        {
            _context.Mesas.Add(mesa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(BuscarPorId), new { id = mesa.Id }, mesa);
        }

        // Endpoint para atualizar dados ou status da mesa (ex: de "Livre" para "Ocupada")
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Mesa mesaAtualizada)
        {
            var mesaExistente = await _context.Mesas.FindAsync(id);

            if (mesaExistente == null)
                return NotFound();

            mesaExistente.Numero = mesaAtualizada.Numero;
            mesaExistente.QuantidadeAssentos = mesaAtualizada.QuantidadeAssentos;
            mesaExistente.Status = mesaAtualizada.Status;

            await _context.SaveChangesAsync();

            return Ok(mesaExistente);
        }

        // Endpoint para excluir uma mesa
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var mesa = await _context.Mesas.FindAsync(id);

            if (mesa == null)
                return NotFound();

            _context.Mesas.Remove(mesa);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}