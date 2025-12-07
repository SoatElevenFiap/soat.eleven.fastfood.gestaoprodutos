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

        public async Task AddAsync(ProdutoDto dto)
        {
            var model = Parse(dto);
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

        public async Task UpdateAsync(ProdutoDto dto)
        {
            var model = await _dbSet.FindAsync(dto.Id);

            if (model == null)
            {
                throw new KeyNotFoundException($"Produto com Id {dto.Id} não encontrado.");
            }

            model.Nome = dto.Nome;
            model.Descricao = dto.Descricao;
            model.Preco = dto.Preco;
            model.CategoriaId = dto.CategoriaId;
            model.Ativo = dto.Ativo;

            _dbSet.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ProdutoDto dto)
        {
            var model = Parse(dto);
            _dbSet.Remove(model);
            await _context.SaveChangesAsync();
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

        private static ProdutoModel Parse(ProdutoDto dto)
        {
            var model = new ProdutoModel
            {
                Id = dto.Id,
                SKU = dto.SKU,
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                Preco = dto.Preco,
                CategoriaId = dto.CategoriaId,
                Ativo = dto.Ativo,
                CriadoEm = dto.CriadoEm,
                Imagem = dto.Imagem
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
                Imagem = model.Imagem,
                CriadoEm = model.CriadoEm
            };
        }
    }
}
