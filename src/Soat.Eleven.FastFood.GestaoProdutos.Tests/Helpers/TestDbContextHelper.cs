using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra.Data;

namespace Soat.Eleven.FastFood.GestaoProdutos.Tests.Helpers;

public class TestDbContextHelper : IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private readonly AppDbContext _context;

    public TestDbContextHelper()
    {
        var services = new ServiceCollection();
        
        services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()));

        _serviceProvider = services.BuildServiceProvider();
        _context = _serviceProvider.GetRequiredService<AppDbContext>();
        
        // Ensure database is created
        _context.Database.EnsureCreated();
    }

    public AppDbContext GetContext() => _context;

    public void ClearDatabase()
    {
        _context.CategoriasProdutos.RemoveRange(_context.CategoriasProdutos);
        _context.Produtos.RemoveRange(_context.Produtos);
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context?.Dispose();
        _serviceProvider?.Dispose();
        GC.SuppressFinalize(this);
    }
}