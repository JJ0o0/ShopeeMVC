using System.Text.Json.Serialization;
namespace ShopeeMVC.Models;
public class CarrinhoItem
{
    public int ProdutoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public int Quantidade { get; set; }
    [JsonIgnore] public decimal Subtotal => Preco * Quantidade;
}
