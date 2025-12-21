using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra.Data.ModelConfiguration.Base;
using Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra.EntityModel;

namespace Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra.Data.ModelConfiguration
{
    public class ProdutoModelConfiguration : EntityBaseModelConfiguration<ProdutoModel>
    {
        public override void Configure(EntityTypeBuilder<ProdutoModel> builder)
        {
            base.Configure(builder);

            builder.ToTable("Produtos");

            builder.Property(p => p.Nome)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.SKU)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.Descricao)
                .HasMaxLength(1000);

            builder.Property(p => p.Preco)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.CategoriaId)
                .IsRequired();

            builder.Property(p => p.Ativo)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(p => p.Imagem)
                .HasMaxLength(500);

            builder.HasOne(p => p.Categoria)
                .WithMany(c => c.Produtos)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);

            builder.HasIndex(p => p.SKU)
                .IsUnique();

            builder.HasIndex(p => p.CategoriaId);
        }
    }
}
