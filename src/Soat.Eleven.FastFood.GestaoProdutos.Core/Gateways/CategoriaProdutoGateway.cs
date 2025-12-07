using Soat.Eleven.FastFood.GestaoProdutos.Core.DTOs.Categorias;
using Soat.Eleven.FastFood.GestaoProdutos.Core.Entities;
using Soat.Eleven.FastFood.GestaoProdutos.Core.Interfaces.DataSources;

namespace Soat.Eleven.FastFood.GestaoProdutos.Core.Gateways
{
    public class CategoriaProdutoGateway
    {
        private ICategoriaProdutoDataSource _categoriaDataSource;

        public CategoriaProdutoGateway(ICategoriaProdutoDataSource dataSource)
        {
            _categoriaDataSource = dataSource;
        }        

        public async Task CriarCategoria(CategoriaProduto novaCategoria)
        {
            var dto = new CategoriaProdutoDto
            {
                Id = novaCategoria.Id,
                Nome = novaCategoria.Nome,
                Descricao = novaCategoria.Descricao,
                Ativo = novaCategoria.Ativo
            };

            await _categoriaDataSource.AddAsync(dto);
        }

        public async Task AtualizarCategoria(CategoriaProduto categoria)
        {
            var dto = new CategoriaProdutoDto
            {
                Id = categoria.Id,
                Nome = categoria.Nome,
                Descricao = categoria.Descricao,
                Ativo = categoria.Ativo
            };

            await _categoriaDataSource.UpdateAsync(dto);
        }

        public async Task<CategoriaProduto?> ObterCategoriaPorId(Guid id)
        {
            var categoriaDto = await _categoriaDataSource.GetByIdAsync(id);

            if (categoriaDto == null)
                return null;

            var categoria = new CategoriaProduto
            {
                Id = categoriaDto.Id,
                Nome = categoriaDto.Nome,
                Descricao = categoriaDto.Descricao,
                Ativo = categoriaDto.Ativo
            };

            return categoria;
        }

        public async Task<IEnumerable<CategoriaProduto>> ListarTodasCategorias()
        {
            var categoriasDto = await _categoriaDataSource.GetAllAsync();

            var categorias = categoriasDto.Select(c => new CategoriaProduto
            {
                Id = c.Id,
                Nome = c.Nome,
                Descricao = c.Descricao,
                Ativo = c.Ativo
            });

            return categorias;
        }

        public async Task<IEnumerable<CategoriaProduto>> ListarCategoriasAtivas()
        {
            var categoriasDto = await _categoriaDataSource.FindAsync(c => c.Ativo);

            var categorias = categoriasDto.Select(c => new CategoriaProduto
            {
                Id = c.Id,
                Nome = c.Nome,
                Descricao = c.Descricao,
                Ativo = c.Ativo
            });

            return categorias;
        }       

        public async Task<bool> CategoriaExiste(string nome)
        {
            var categorias = await _categoriaDataSource.FindAsync(c => c.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));
            return categorias.Any();
        }        
    }
}