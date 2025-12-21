using Microsoft.EntityFrameworkCore;
using Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra.Data;
using Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra.EntityModel;
using Soat.Eleven.FastFood.GestaoProdutos.Core.DTOs.Categorias;
using Soat.Eleven.FastFood.GestaoProdutos.Core.Interfaces.DataSources;

namespace Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra.DataSources
{
    public class CategoriaProdutoDataSource : ICategoriaProdutoDataSource
    {
        private readonly AppDbContext _context;
        private readonly DbSet<CategoriaProdutoModel> _dbSet;

        public CategoriaProdutoDataSource(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<CategoriaProdutoModel>();
        }

        public async Task<CategoriaProdutoDto> AddAsync(CategoriaProdutoDto categoria)
        {
            var model = Parse(categoria);
            await _dbSet.AddAsync(model);
            await _context.SaveChangesAsync();

            return categoria;
        }

        public async Task<CategoriaProdutoDto?> GetByIdAsync(Guid id)
        {
            var result = await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
            return result != null ? Parse(result) : null;
        }

        public async Task<IEnumerable<CategoriaProdutoDto>> GetAllAsync()
        {
            var result = await _dbSet.AsNoTracking().ToListAsync();
            return result.Select(Parse);
        }

        public async Task<IEnumerable<CategoriaProdutoDto>> FindAsync(Func<CategoriaProdutoDto, bool> predicate)
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

        public async Task<CategoriaProdutoDto> UpdateAsync(CategoriaProdutoDto categoria)
        {
            var model = await _dbSet.FindAsync(categoria.Id);

            if (model == null)
            {
                throw new KeyNotFoundException($"Categoria com Id {categoria.Id} não encontrada.");
            }

            model.Nome = categoria.Nome;
            model.Descricao = categoria.Descricao;
            model.Ativo = categoria.Ativo;

            _dbSet.Update(model);
            await _context.SaveChangesAsync();

            return Parse(model);
        }

        public async Task DeleteAsync(CategoriaProdutoDto categoria)
        {
            var model = await _dbSet.FindAsync(categoria.Id);
            
            if (model != null)
            {
                _dbSet.Remove(model);
                await _context.SaveChangesAsync();
            }
        }

        private static CategoriaProdutoModel Parse(CategoriaProdutoDto categoria)
        {
            var model = new CategoriaProdutoModel
            {
                Id = categoria.Id,
                Nome = categoria.Nome,
                Descricao = categoria.Descricao,
                Ativo = categoria.Ativo
            };

            return model;
        }

        private static CategoriaProdutoDto Parse(CategoriaProdutoModel categoria)
        {
            return new CategoriaProdutoDto
            {
                Id = categoria.Id,
                Nome = categoria.Nome,
                Descricao = categoria.Descricao,
                Ativo = categoria.Ativo
            };
        }
    }
}
