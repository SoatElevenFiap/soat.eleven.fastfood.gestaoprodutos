namespace Soat.Eleven.FastFood.GestaoProdutos.Core.DTOs.Images;

public class ImagemProdutoArquivo
{
    public string Nome { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public Stream Conteudo { get; set; } = null!;
}
