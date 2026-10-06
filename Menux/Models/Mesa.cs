using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Menux.Models
{
    public class Mesa
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O número da mesa é obrigatório.")]
        [Range(1, 500, ErrorMessage = "O número da mesa deve estar entre 1 e 500.")]
        public int Numero { get; set; }

        [Required(ErrorMessage = "A quantidade de assentos é obrigatória.")]
        [Range(1, 20, ErrorMessage = "A mesa deve ter capacidade entre 1 e 20 assentos.")]
        public int QuantidadeAssentos { get; set; }

        [Required(ErrorMessage = "O status da mesa é obrigatório.")]
        [RegularExpression("^(Livre|Ocupada|Aguardando Conta)$",
            ErrorMessage = "O status deve ser 'Livre', 'Ocupada' ou 'Aguardando Conta'.")]
        public string Status { get; set; } = "Livre";

        // Relacionamento 1:N com PedidoItem
        [JsonIgnore]
        public List<PedidoItem> Pedidos { get; set; } = new();
    }
}