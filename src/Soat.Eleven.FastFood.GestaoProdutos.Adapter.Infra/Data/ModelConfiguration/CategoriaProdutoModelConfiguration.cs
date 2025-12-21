using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra.Data.ModelConfiguration.Base;
using Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra.EntityModel;

namespace Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra.Data.ModelConfiguration
{
    public class CategoriaProdutoModelConfiguration : EntityBaseModelConfiguration<CategoriaProdutoModel>
    {
        public override void Configure(EntityTypeBuilder<CategoriaProdutoModel> builder)
        {
            base.Configure(builder);

            builder.ToTable("CategoriaProdutos");

            builder.Property(c => c.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Descricao)
                .HasMaxLength(500);

            builder.Property(c => c.Ativo)
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasMany(c => c.Produtos)
                .WithOne(p => p.Categoria)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);

            builder.HasIndex(c => c.Nome)
                .IsUnique();
        }
    }
}