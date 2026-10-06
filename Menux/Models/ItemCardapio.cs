using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Menux.Models
{
    public class ItemCardapio
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do item é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(300, ErrorMessage = "A descrição não pode exceder 300 caracteres.")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O preço é obrigatório.")]
        [Range(0.01, 1000.00, ErrorMessage = "O preço deve ser maior que zero e até R$ 1.000,00.")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "A categoria é obrigatória.")]
        [RegularExpression("^(Entrada|Prato Principal|Bebida|Sobremesa)$",
            ErrorMessage = "Categoria permitida: 'Entrada', 'Prato Principal', 'Bebida' ou 'Sobremesa'.")]
        public string Categoria { get; set; } = string.Empty;

        public bool Disponivel { get; set; } = true;

        [JsonIgnore]
        public List<PedidoItem> Pedidos { get; set; } = new();
    }
}