using Microsoft.EntityFrameworkCore;
namespace ShopeeMVC.Models;
public partial class DbShopeeContext : DbContext
{
    public DbShopeeContext(DbContextOptions<DbShopeeContext> options) : base(options) { }
    public virtual DbSet<Produto> Produtos { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Produto>(entity => {
            entity.HasKey(e => e.Codigo);
            entity.ToTable("Produto");
            entity.Property(e => e.Nome).HasMaxLength(100).IsUnicode(false).IsRequired();
            entity.Property(e => e.Preco).HasColumnType("decimal(10, 2)");
        });
        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
