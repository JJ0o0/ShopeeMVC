using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopeeMVC.Models;
using ShopeeMVC.ViewModels;
namespace ShopeeMVC.Controllers;
public class ProdutoController(DbShopeeContext context) : Controller
{
    public async Task<IActionResult> Index() => View(await context.Produtos.AsNoTracking().OrderBy(p => p.Codigo).ToListAsync());
    public async Task<IActionResult> Details(int id) {
        var produto = await context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.Codigo == id);
        return produto == null ? NotFound() : View(produto);
    }
    public IActionResult Create() => View(new ProdutoForm());
    [HttpPost]
    public async Task<IActionResult> Create(ProdutoForm form) {
        Normalizar(form);
        if (!ModelState.IsValid) return View(form);
        context.Produtos.Add(new Produto { Nome = form.Nome, Preco = form.Preco, Estoque = form.Estoque });
        await context.SaveChangesAsync();
        TempData["Sucesso"] = "Produto cadastrado.";
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Edit(int id) {
        var p = await context.Produtos.FindAsync(id);
        return p == null ? NotFound() : View(new ProdutoForm { Codigo = p.Codigo, Nome = p.Nome, Preco = p.Preco, Estoque = p.Estoque });
    }
    [HttpPost]
    public async Task<IActionResult> Edit(int id, ProdutoForm form) {
        if (id != form.Codigo) return BadRequest();
        Normalizar(form);
        if (!ModelState.IsValid) return View(form);
        var p = await context.Produtos.FindAsync(id);
        if (p == null) return NotFound();
        p.Nome = form.Nome; p.Preco = form.Preco; p.Estoque = form.Estoque;
        try { await context.SaveChangesAsync(); }
        catch (DbUpdateConcurrencyException) { if (!await context.Produtos.AnyAsync(x => x.Codigo == id)) return NotFound(); throw; }
        TempData["Sucesso"] = "Produto atualizado.";
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int id) {
        var produto = await context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.Codigo == id);
        return produto == null ? NotFound() : View(produto);
    }
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id) {
        var p = await context.Produtos.FindAsync(id);
        if (p == null) return NotFound();
        context.Produtos.Remove(p); await context.SaveChangesAsync();
        TempData["Sucesso"] = "Produto excluído.";
        return RedirectToAction(nameof(Index));
    }
    private void Normalizar(ProdutoForm form) {
        form.Nome = (form.Nome ?? "").Trim();
        if (string.IsNullOrWhiteSpace(form.Nome)) ModelState.AddModelError(nameof(form.Nome), "Informe o nome.");
        if (decimal.Round(form.Preco, 2) != form.Preco) ModelState.AddModelError(nameof(form.Preco), "Use no máximo duas casas decimais.");
    }
}
