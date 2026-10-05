namespace ShopeeMVC.Models;
// Entidade correspondente à tabela dbo.Produto do script Database First.
public partial class Produto
{
    public int Codigo { get; set; }
    public string Nome { get; set; } = null!;
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
}
