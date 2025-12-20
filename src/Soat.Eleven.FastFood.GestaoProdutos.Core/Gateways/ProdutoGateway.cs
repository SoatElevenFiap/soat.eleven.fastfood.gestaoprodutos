using Soat.Eleven.FastFood.GestaoProdutos.Core.DTOs.Produtos;
using Soat.Eleven.FastFood.GestaoProdutos.Core.Entities;
using Soat.Eleven.FastFood.GestaoProdutos.Core.Interfaces.DataSources;
using Soat.Eleven.FastFood.GestaoProdutos.Core.Interfaces.Gateways;

namespace Soat.Eleven.FastFood.GestaoProdutos.Core.Gateways
{
    public class ProdutoGateway : IProdutoGateway
    {
        private readonly IProdutoDataSource _produtoDataSource;        

        public ProdutoGateway(IProdutoDataSource dataSource)
        {
            _produtoDataSource = dataSource;
        }

        public async Task CriarProduto(Produto produto)
        {
            var dto = new ProdutoDto
            {
                Id = produto.Id,
                Nome = produto.Nome,
                SKU = produto.SKU,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                CategoriaId = produto.CategoriaId,
                Ativo = produto.Ativo,
                Imagem = produto.Imagem
            };

            await _produtoDataSource.AddAsync(dto);
        }

        public async Task AtualizarProduto(Produto produto)
        {
            var dto = new ProdutoDto
            {
                Id = produto.Id,
                Nome = produto.Nome,
                SKU = produto.SKU,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                CategoriaId = produto.CategoriaId,
                Ativo = produto.Ativo,
                Imagem = produto.Imagem
            };

            await _produtoDataSource.UpdateAsync(dto);
        }

        public async Task<Produto?> ObterProdutoPorId(Guid id)
        {
            var produtoDto = await _produtoDataSource.GetByIdAsync(id);

            if (produtoDto == null)
                return null;

            var produto = new Produto
            {
                Id = produtoDto.Id,
                Nome = produtoDto.Nome,
                SKU = produtoDto.SKU,
                Descricao = produtoDto.Descricao,
                Preco = produtoDto.Preco,
                CategoriaId = produtoDto.CategoriaId,
                Ativo = produtoDto.Ativo,
                Imagem = produtoDto.Imagem
            };

            return produto;
        }

        public async Task<IEnumerable<Produto>> ListarTodosProdutos()
        {
            var produtosDto = await _produtoDataSource.GetAllAsync();

            var produtos = produtosDto.Select(p => new Produto
            {
                Id = p.Id,
                Nome = p.Nome,
                SKU = p.SKU,
                Descricao = p.Descricao,
                Preco = p.Preco,
                CategoriaId = p.CategoriaId,
                Ativo = p.Ativo,
                Imagem = p.Imagem
            });

            return produtos;
        }

        public async Task<IEnumerable<Produto>> ListarProdutosAtivos()
        {
            var produtos = await ListarTodosProdutos();

            return produtos.Where(e => e.Ativo);
        }        

        public async Task<IEnumerable<Produto>> ListarProdutosPorCategoria(Guid categoriaId)
        {
            var produtosDto = await _produtoDataSource.FindAsync(p => p.CategoriaId == categoriaId);

            var produtos = produtosDto.Select(p => new Produto
            {
                Id = p.Id,
                Nome = p.Nome,
                SKU = p.SKU,
                Descricao = p.Descricao,
                Preco = p.Preco,
                CategoriaId = p.CategoriaId,
                Ativo = p.Ativo,
                Imagem = p.Imagem
            });

            return produtos;
        }

        public async Task<IEnumerable<Produto>> ListarProdutosAtivosPorCategoria(Guid categoriaId)
        {
            var produtos = await ListarProdutosPorCategoria(categoriaId);

            return produtos.Where(e => e.Ativo);
        }

        public async Task<bool> ProdutoExiste(string sku)
        {
            var produtos = await _produtoDataSource.FindAsync(e=> e.SKU == sku);
            return produtos.Any();
        }
    }
}
