using Microsoft.EntityFrameworkCore;
using Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra.Data;
using Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra.EntityModel;
using Soat.Eleven.FastFood.GestaoProdutos.Core.DTOs.Produtos;
using Soat.Eleven.FastFood.GestaoProdutos.Core.Interfaces.DataSources;

namespace Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra.DataSources
{
    public class ProdutoDataSource : IProdutoDataSource
    {
        private readonly AppDbContext _context;
        private readonly DbSet<ProdutoModel> _dbSet;

        public ProdutoDataSource(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<ProdutoModel>();
        }

        public async Task AddAsync(ProdutoDto produto)
        {
            var model = Parse(produto);
            await _dbSet.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task<ProdutoDto?> GetByIdAsync(Guid id)
        {
            var result = await _dbSet
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(e => e.Id == id);

            return result != null ? Parse(result) : null;
        }

        public async Task<IEnumerable<ProdutoDto>> GetAllAsync()
        {
            var result = await _dbSet
                .Include(p => p.Categoria)
                .AsNoTracking()
                .ToListAsync();

            return result.Select(Parse);
        }

        public async Task<IEnumerable<ProdutoDto>> FindAsync(Func<ProdutoDto, bool> predicate)
        {
            var result = await _dbSet
                .AsSplitQuery()
                .AsNoTracking()
                .ToListAsync();

            var entities = result.Select(Parse);

            if (predicate != null)
            {
                entities = entities.AsQueryable().Where(predicate);
            }

            return entities;
        }

        public async Task UpdateAsync(ProdutoDto produto)
        {
            var model = await _dbSet.FindAsync(produto.Id);

            if (model == null)
            {
                throw new KeyNotFoundException($"Produto com Id {produto.Id} não encontrado.");
            }

            model.Nome = produto.Nome;
            model.Descricao = produto.Descricao;
            model.Preco = produto.Preco;
            model.CategoriaId = produto.CategoriaId;
            model.Ativo = produto.Ativo;

            _dbSet.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ProdutoDto produto)
        {
            var model = await _dbSet.FindAsync(produto.Id);
            if (model != null)
            {
                model.Ativo = false;
                _dbSet.Update(model);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ProdutoDto>> GetByCategoriaAsync(Guid categoriaId)
        {
            var result = await _dbSet
                .Include(p => p.Categoria)
                .Where(p => p.CategoriaId == categoriaId)
                .AsNoTracking()
                .ToListAsync();

            return result.Select(Parse);
        }

        private static ProdutoModel Parse(ProdutoDto produto)
        {
            var model = new ProdutoModel
            {
                Id = produto.Id,
                SKU = produto.SKU,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                CategoriaId = produto.CategoriaId,
                Ativo = produto.Ativo,
                Imagem = produto.Imagem
            };
            return model;
        }

        private static ProdutoDto Parse(ProdutoModel model)
        {
            return new ProdutoDto
            {
                Id = model.Id,
                Nome = model.Nome,
                Descricao = model.Descricao,
                Preco = model.Preco,
                CategoriaId = model.CategoriaId,
                Ativo = model.Ativo,
                SKU = model.SKU,
                Imagem = model.Imagem
            };
        }
    }
}
