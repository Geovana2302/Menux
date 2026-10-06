using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Menux.Models
{
    public class PedidoItem
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "A identificação da mesa é obrigatória.")]
        public int MesaId { get; set; }
        public Mesa? Mesa { get; set; }

        [Required(ErrorMessage = "A identificação do item do cardápio é obrigatória.")]
        public int ItemCardapioId { get; set; }
        public ItemCardapio? ItemCardapio { get; set; }

        [Required(ErrorMessage = "A quantidade é obrigatória.")]
        [Range(1, 50, ErrorMessage = "A quantidade deve ser de no mínimo 1 e no máximo 50 unidades.")]
        public int Quantidade { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecoUnitario { get; set; }

        [StringLength(200, ErrorMessage = "A observação não pode exceder 200 caracteres.")]
        public string Observacao { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^(Recebido|Em Preparo|Pronto|Entregue)$",
            ErrorMessage = "Status permitido: 'Recebido', 'Em Preparo', 'Pronto' ou 'Entregue'.")]
        public string StatusPreparo { get; set; } = "Recebido";

        public DateTime DataHoraSolicitacao { get; set; } = DateTime.Now;
    }
}