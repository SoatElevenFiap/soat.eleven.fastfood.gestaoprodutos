using Soat.Eleven.FastFood.GestaoProdutos.Core.DTOs.Images;
using Soat.Eleven.FastFood.GestaoProdutos.Core.DTOs.Produtos;
using Soat.Eleven.FastFood.GestaoProdutos.Core.Entities;
using Soat.Eleven.FastFood.GestaoProdutos.Core.Gateways;
using Soat.Eleven.FastFood.GestaoProdutos.Core.Interfaces.Services;

namespace Soat.Eleven.FastFood.Core.UseCases
{
    public class ProdutoUseCase
    {
        private readonly ProdutoGateway _produtoGateway;
        private readonly CategoriaProdutoGateway _categoriaProdutoGateway;
        private const string DIRETORIO_IMAGENS = "produtos";

        private ProdutoUseCase(
            ProdutoGateway produtoGateway,
            CategoriaProdutoGateway categoriaProdutoGateway)
        {
            _produtoGateway = produtoGateway;
            _categoriaProdutoGateway = categoriaProdutoGateway;
        }

        public static ProdutoUseCase Create(
            ProdutoGateway produtoGateway,
            CategoriaProdutoGateway categoriaProdutoGateway)
        {
            return new ProdutoUseCase(produtoGateway, categoriaProdutoGateway);
        }

        private async Task<string> ObterUrlCompleta(string nomeImagem, IArmazenamentoArquivoGateway armazenamentoArquivoGateway)
        {
            return await armazenamentoArquivoGateway.ObterUrlImagemAsync(DIRETORIO_IMAGENS, nomeImagem);
        }

        public async Task<IEnumerable<Produto>> ListarProdutos(bool? incluirInativos = false, Guid? categoryId = null)
        {
            IEnumerable<Produto> produtos;

            if (categoryId.HasValue)
            {
                var categoria = await _categoriaProdutoGateway.ObterCategoriaPorId(categoryId.Value);

                if (categoria == null)
                    throw new ArgumentException("Categoria não encontrada");

                produtos = incluirInativos == true
                    ? await _produtoGateway.ListarProdutosPorCategoria(categoryId.Value)
                    : await _produtoGateway.ListarProdutosAtivosPorCategoria(categoryId.Value);
            }
            else
            {
                produtos = incluirInativos == true
                    ? await _produtoGateway.ListarTodosProdutos()
                    : await _produtoGateway.ListarProdutosAtivos();
            }

            return produtos;
        }

        public async Task<Produto?> ObterProdutoPorId(Guid id)
        {
            var produto = await _produtoGateway.ObterProdutoPorId(id);

            if (produto is null)
                throw new KeyNotFoundException();

            return produto;
        }

        public async Task<Produto> CriarProduto(CriarProdutoDto produtoDto)
        {

            var produto = new Produto
            {
                Id = Guid.NewGuid(),
                Nome = produtoDto.Nome,
                SKU = produtoDto.SKU,
                Descricao = produtoDto.Descricao,
                Preco = produtoDto.Preco,
                CategoriaId = produtoDto.CategoriaId,
                Ativo = true,
                CriadoEm = DateTime.UtcNow,
                Imagem = produtoDto.Imagem
            };


            if (produtoDto.Preco <= 0)
                throw new ArgumentException("O preço do produto deve ser maior que zero");

            var existeProduto = await _produtoGateway.ProdutoExiste(produtoDto.SKU);
            if (existeProduto)
                throw new ArgumentException("Produto com mesmo SKU já existe");

            var categoria = await _categoriaProdutoGateway.ObterCategoriaPorId(produtoDto.CategoriaId);
            if (categoria == null)
                throw new ArgumentException("Categoria não encontrada");


            await _produtoGateway.CriarProduto(produto);

            return produto;
        }

        public async Task<Produto> AtualizarProduto(AtualizarProdutoDto produtoDto)
        {
            var produto = new Produto
            {
                Id = Guid.NewGuid(),
                Nome = produtoDto.Nome,
                SKU = produtoDto.SKU,
                Descricao = produtoDto.Descricao,
                Preco = produtoDto.Preco,
                CategoriaId = produtoDto.CategoriaId,
                Ativo = true,
                CriadoEm = DateTime.UtcNow,
                Imagem = produtoDto.Imagem
            };

            var produtoExistente = await _produtoGateway.ObterProdutoPorId(produto.Id);
            if (produtoExistente == null)
                throw new KeyNotFoundException("Produto não encontrado");

            if (produto.Preco <= 0)
                throw new ArgumentException("O preço do produto deve ser maior que zero");

            produtoExistente.Nome = produto.Nome;
            produtoExistente.Descricao = produto.Descricao;
            produtoExistente.Preco = produto.Preco;

            await _produtoGateway.AtualizarProduto(produtoExistente);

            return produtoExistente;
        }

        public async Task DesativarProduto(Guid id)
        {
            var produto = await _produtoGateway.ObterProdutoPorId(id);
            if (produto == null)
                throw new ArgumentException("Produto não encontrado");

            produto.Ativo = false;
            await _produtoGateway.AtualizarProduto(produto);
        }

        public async Task ReativarProduto(Guid id)
        {
            var produto = await _produtoGateway.ObterProdutoPorId(id);
            if (produto == null)
                throw new ArgumentException("Produto não encontrado");

            produto.Ativo = true;
            await _produtoGateway.AtualizarProduto(produto);
        }

        public async Task<string> UploadImagemAsync(Guid produtoId, ImagemProdutoArquivo imagem, IArmazenamentoArquivoGateway armazenamentoArquivoGateway)
        {
            var produto = await _produtoGateway.ObterProdutoPorId(produtoId);
            if (produto == null)
                throw new ArgumentException("Produto não encontrado");

            var nomeArquivo = await armazenamentoArquivoGateway.UploadImagemAsync(DIRETORIO_IMAGENS, produtoId.ToString(), imagem);
            produto.Imagem = nomeArquivo;
            await _produtoGateway.AtualizarProduto(produto);
            return nomeArquivo;
        }

        public async Task RemoverImagemAsync(Guid produtoId, IArmazenamentoArquivoGateway armazenamentoArquivoGateway)
        {
            var produto = await _produtoGateway.ObterProdutoPorId(produtoId);
            if (produto == null)
                throw new ArgumentException("Produto não encontrado");

            await armazenamentoArquivoGateway.RemoverImagemAsync(DIRETORIO_IMAGENS, produtoId.ToString());
            produto.Imagem = null;
            await _produtoGateway.AtualizarProduto(produto);
        }
    }
}