namespace Menux.Models
{
    public class ItemCardapio
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public string Categoria { get; set; } = string.Empty; // Ex: "Lanche", "Bebida", "Sobremesa"
        public bool Disponivel { get; set; } = true;
    }
}