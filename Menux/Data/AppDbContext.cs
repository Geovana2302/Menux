using Menux.Models;
using Microsoft.EntityFrameworkCore;

namespace Menux.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Mesa> Mesas { get; set; }
        public DbSet<ItemCardapio> ItensCardapio { get; set; }
        public DbSet<PedidoItem> PedidosItens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relacionamento Mesa -> PedidoItem (1:N)
            modelBuilder.Entity<PedidoItem>()
                .HasOne(p => p.Mesa)
                .WithMany(m => m.Pedidos)
                .HasForeignKey(p => p.MesaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento ItemCardapio -> PedidoItem (1:N)
            modelBuilder.Entity<PedidoItem>()
                .HasOne(p => p.ItemCardapio)
                .WithMany(i => i.Pedidos)
                .HasForeignKey(p => p.ItemCardapioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}