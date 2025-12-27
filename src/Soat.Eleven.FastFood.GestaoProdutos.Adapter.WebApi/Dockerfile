# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Soat.Eleven.FastFood.GestaoProdutos.Adapter.WebApi/Soat.Eleven.FastFood.GestaoProdutos.Adapter.WebApi.csproj", "Soat.Eleven.FastFood.GestaoProdutos.Adapter.WebApi/"]
COPY ["Soat.Eleven.FastFood.GestaoProdutos.Core/Soat.Eleven.FastFood.GestaoProdutos.Core.csproj", "Soat.Eleven.FastFood.GestaoProdutos.Core/"]
COPY ["Soat.Eleven.FastFood.GestaoProdutos.Application/Soat.Eleven.FastFood.GestaoProdutos.Application.csproj", "Soat.Eleven.FastFood.GestaoProdutos.Application/"]
COPY ["Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra/Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra.csproj", "Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra/"]
RUN dotnet restore "./Soat.Eleven.FastFood.GestaoProdutos.Adapter.WebApi/Soat.Eleven.FastFood.GestaoProdutos.Adapter.WebApi.csproj"
COPY ["Soat.Eleven.FastFood.GestaoProdutos.Adapter.WebApi/", "Soat.Eleven.FastFood.GestaoProdutos.Adapter.WebApi/"]
COPY ["Soat.Eleven.FastFood.GestaoProdutos.Core/", "Soat.Eleven.FastFood.GestaoProdutos.Core/"]
COPY ["Soat.Eleven.FastFood.GestaoProdutos.Application/", "Soat.Eleven.FastFood.GestaoProdutos.Application/"]
COPY ["Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra/", "Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra/"]
WORKDIR "/src/Soat.Eleven.FastFood.GestaoProdutos.Adapter.WebApi"
RUN dotnet build "./Soat.Eleven.FastFood.GestaoProdutos.Adapter.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Soat.Eleven.FastFood.GestaoProdutos.Adapter.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada para executar migrations no banco de dados
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS migrator
WORKDIR /src
COPY --from=build /src /src
WORKDIR "/src/Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra"
ENTRYPOINT ["dotnet", "ef", "database", "update", "--project", ".", "--startup-project", "../Soat.Eleven.FastFood.GestaoProdutos.Adapter.WebApi"]

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
USER $APP_UID
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Soat.Eleven.FastFood.GestaoProdutos.Adapter.WebApi.dll"]