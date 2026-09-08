namespace Menux.Models
{
    public class PedidoItem
    {
        public int Id { get; set; }

        // Chave estrangeira e navegação da Mesa
        public int MesaId { get; set; }
        public Mesa? Mesa { get; set; }

        // Chave estrangeira e navegação do ItemCardapio
        public int ItemCardapioId { get; set; }
        public ItemCardapio? ItemCardapio { get; set; }

        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public string Observacao { get; set; } = string.Empty; // Ex: "Sem cebola", "Bem passado"

        // Ciclo de vida na cozinha: "Recebido", "Em Preparo", "Pronto", "Entregue"
        public string StatusPreparo { get; set; } = "Recebido";
        public DateTime DataHoraSolicitacao { get; set; } = DateTime.Now;
    }
}