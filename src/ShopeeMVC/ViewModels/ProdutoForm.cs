using System.ComponentModel.DataAnnotations;
namespace ShopeeMVC.ViewModels;
// Validação separada para não ser perdida ao refazer o scaffold das entidades.
public class ProdutoForm
{
    public int Codigo { get; set; }
    [Required(ErrorMessage = "Informe o nome.")]
    [StringLength(100, ErrorMessage = "Use no máximo 100 caracteres.")]
    [Display(Name = "Nome do produto")]
    public string Nome { get; set; } = string.Empty;
    [Range(typeof(decimal), "0.01", "99999999.99", ParseLimitsInInvariantCulture = true, ConvertValueInInvariantCulture = true, ErrorMessage = "Informe um preço entre 0,01 e 99.999.999,99.")]
    [Display(Name = "Preço (R$)")]
    public decimal Preco { get; set; }
    [Range(0, 1000000, ErrorMessage = "Estoque deve estar entre 0 e 1.000.000.")]
    public int Estoque { get; set; }
}
