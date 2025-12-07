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

        public async Task<CategoriaProdutoDto> AddAsync(CategoriaProdutoDto dto)
        {
            var model = Parse(dto);
            await _dbSet.AddAsync(model);
            await _context.SaveChangesAsync();

            return dto;
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

        public async Task<CategoriaProdutoDto> UpdateAsync(CategoriaProdutoDto dto)
        {
            var model = await _dbSet.FindAsync(dto.Id);

            if (model == null)
            {
                throw new KeyNotFoundException($"Categoria com Id {dto.Id} não encontrada.");
            }

            model.Nome = dto.Nome;
            model.Descricao = dto.Descricao;
            model.Ativo = dto.Ativo;

            _dbSet.Update(model);
            await _context.SaveChangesAsync();

            return Parse(model);
        }

        public async Task DeleteAsync(CategoriaProdutoDto dto)
        {
            var model = Parse(dto);
            _dbSet.Remove(model);
            await _context.SaveChangesAsync();
        }

        private static CategoriaProdutoModel Parse(CategoriaProdutoDto dto)
        {
            var model = new CategoriaProdutoModel
            {
                Id = dto.Id,
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                Ativo = dto.Ativo
            };

            return model;
        }

        private static CategoriaProdutoDto Parse(CategoriaProdutoModel model)
        {
            return new CategoriaProdutoDto
            {
                Id = model.Id,
                Nome = model.Nome,
                Descricao = model.Descricao,
                Ativo = model.Ativo
            };
        }
    }
}
