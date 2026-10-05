using System.Text.Json;
using ShopeeMVC.Models;
namespace ShopeeMVC.Services;
public class CarrinhoService
{
    public const string Chave = "Carrinho";
    public List<CarrinhoItem> ObterCarrinho(ISession session)
    {
        var json = session.GetString(Chave);
        return string.IsNullOrEmpty(json) ? new() : JsonSerializer.Deserialize<List<CarrinhoItem>>(json) ?? new();
    }
    public void SalvarCarrinho(ISession session, List<CarrinhoItem> itens) => session.SetString(Chave, JsonSerializer.Serialize(itens));
    public string? Adicionar(ISession session, Produto produto)
    {
        var itens = ObterCarrinho(session);
        var item = itens.FirstOrDefault(x => x.ProdutoId == produto.Codigo);
        if (produto.Estoque <= (item?.Quantidade ?? 0)) return "Quantidade disponível em estoque atingida.";
        if (item == null) itens.Add(new CarrinhoItem { ProdutoId = produto.Codigo, Nome = produto.Nome, Preco = produto.Preco, Quantidade = 1 });
        else { item.Quantidade++; item.Nome = produto.Nome; item.Preco = produto.Preco; }
        SalvarCarrinho(session, itens);
        return null;
    }
    public string? Atualizar(ISession session, Produto produto, int quantidade)
    {
        if (quantidade < 1 || quantidade > produto.Estoque) return "Informe uma quantidade positiva dentro do estoque disponível.";
        var itens = ObterCarrinho(session);
        var item = itens.FirstOrDefault(x => x.ProdutoId == produto.Codigo);
        if (item == null) return "Produto não encontrado no carrinho.";
        item.Quantidade = quantidade; item.Nome = produto.Nome; item.Preco = produto.Preco;
        SalvarCarrinho(session, itens);
        return null;
    }
    public void Remover(ISession session, int id)
    {
        var itens = ObterCarrinho(session);
        itens.RemoveAll(x => x.ProdutoId == id);
        SalvarCarrinho(session, itens);
    }
}
