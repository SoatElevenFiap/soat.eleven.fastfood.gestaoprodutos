namespace Soat.Eleven.FastFood.GestaoProdutos.Core.DTOs.Categorias
{
    public class CategoriaProdutoDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = null!;
        public string? Descricao { get; set; }
        public bool Ativo { get; set; }
    }
}