using Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra.DataSources;
using Soat.Eleven.FastFood.GestaoProdutos.Core.Interfaces.DataSources;

namespace Soat.Eleven.FastFood.GestaoProdutos.Adapter.WebApi.Configuration;

public static class RegisterServicesConfiguration
{
    public static void RegisterServices(this IServiceCollection serviceCollection)
    {
        #region Data Sources
        serviceCollection.AddScoped<ICategoriaProdutoDataSource, CategoriaProdutoDataSource>();
        serviceCollection.AddScoped<IProdutoDataSource, ProdutoDataSource>();
        #endregion       
    }
}
