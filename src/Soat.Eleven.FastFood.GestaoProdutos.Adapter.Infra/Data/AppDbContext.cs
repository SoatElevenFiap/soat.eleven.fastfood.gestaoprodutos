using Microsoft.EntityFrameworkCore;
using Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra.Data.ModelConfiguration;
using Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra.EntityModel;

namespace Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<CategoriaProdutoModel> CategoriasProdutos { get; set; }
        public DbSet<ProdutoModel> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoriaProdutoModelConfiguration());
            modelBuilder.ApplyConfiguration(new ProdutoModelConfiguration());
        }
    }
}