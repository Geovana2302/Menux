namespace Menux.Models
{
    public class Mesa
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public int QuantidadeAssentos { get; set; }

        // Ex: "Livre", "Ocupada", "Aguardando Conta"
        public string Status { get; set; } = "Livre";

        // Relacionamento: uma mesa pode ter vários pedidos
        public List<PedidoItem> Pedidos { get; set; } = new();
    }
}