using System.Collections.Generic;
using Menux.Models;
using Microsoft.EntityFrameworkCore;

namespace Menux.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Mesa> Mesas { get; set; }
        public DbSet<ItemCardapio> ItensCardapio { get; set; }
        public DbSet<PedidoItem> PedidosItens { get; set; }
    }
}