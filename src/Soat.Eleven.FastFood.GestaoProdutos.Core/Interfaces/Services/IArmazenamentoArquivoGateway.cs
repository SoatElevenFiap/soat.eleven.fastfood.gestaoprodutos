using Soat.Eleven.FastFood.GestaoProdutos.Core.DTOs.Images;

namespace Soat.Eleven.FastFood.GestaoProdutos.Core.Interfaces.Services;

public interface IArmazenamentoArquivoGateway
{
    Task<string> UploadImagemAsync(string diretorio, string identificador, ImagemProdutoArquivo imagem);
    Task RemoverImagemAsync(string diretorio, string identificador);
    Task<string> ObterUrlImagemAsync(string diretorio, string nomeArquivo);
}
