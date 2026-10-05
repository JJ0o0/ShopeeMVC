using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopeeMVC.Models;
using ShopeeMVC.Services;
namespace ShopeeMVC.Controllers;
public class CarrinhoController(DbShopeeContext context, CarrinhoService carrinho) : Controller
{
    public async Task<IActionResult> Index() {
        var itens = carrinho.ObterCarrinho(HttpContext.Session);
        var ids = itens.Select(x => x.ProdutoId).ToList();
        if (ids.Count > 0) {
            var produtos = await context.Produtos.AsNoTracking().Where(x => ids.Contains(x.Codigo)).ToDictionaryAsync(x => x.Codigo);
            var ajustado = false;
            foreach (var item in itens.ToList()) {
                if (!produtos.TryGetValue(item.ProdutoId, out var p) || p.Estoque <= 0) { itens.Remove(item); ajustado = true; continue; }
                if (item.Quantidade > p.Estoque) { item.Quantidade = p.Estoque; ajustado = true; }
                if (item.Preco != p.Preco || item.Nome != p.Nome) ajustado = true;
                item.Preco = p.Preco; item.Nome = p.Nome;
            }
            carrinho.SalvarCarrinho(HttpContext.Session, itens);
            if (ajustado) TempData["Aviso"] = "Carrinho atualizado conforme preços e estoque atuais.";
        }
        return View(itens);
    }
    [HttpPost]
    public async Task<IActionResult> Adicionar(int id) {
        var p = await context.Produtos.AsNoTracking().FirstOrDefaultAsync(x => x.Codigo == id);
        if (p == null) return NotFound();
        var erro = carrinho.Adicionar(HttpContext.Session, p);
        TempData[erro == null ? "Sucesso" : "Aviso"] = erro ?? "Produto adicionado ao carrinho.";
        return RedirectToAction(nameof(Index));
    }
    [HttpPost]
    public async Task<IActionResult> Atualizar(int id, int quantidade) {
        var p = await context.Produtos.AsNoTracking().FirstOrDefaultAsync(x => x.Codigo == id);
        if (p == null) { carrinho.Remover(HttpContext.Session, id); TempData["Aviso"] = "Produto indisponível e removido do carrinho."; }
        else {
            var erro = carrinho.Atualizar(HttpContext.Session, p, quantidade);
            TempData[erro == null ? "Sucesso" : "Aviso"] = erro ?? "Quantidade atualizada.";
        }
        return RedirectToAction(nameof(Index));
    }
    [HttpPost]
    public IActionResult Remover(int id) { carrinho.Remover(HttpContext.Session, id); TempData["Sucesso"] = "Item removido."; return RedirectToAction(nameof(Index)); }
    [HttpPost]
    public IActionResult Limpar() { HttpContext.Session.Remove(CarrinhoService.Chave); TempData["Sucesso"] = "Carrinho esvaziado."; return RedirectToAction(nameof(Index)); }
}
