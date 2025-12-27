FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /app

COPY src/*.sln src/
COPY src/Soat.Eleven.GestaoProdutos.Tests/*.csproj src/Soat.Eleven.GestaoProdutos.Tests/
COPY src/Soat.Eleven.GestaoProdutos.Api/*.csproj src/Soat.Eleven.GestaoProdutos.Api/
COPY src/Soat.Eleven.GestaoProdutos.Application/*.csproj src/Soat.Eleven.GestaoProdutos.Application/
COPY src/Soat.Eleven.GestaoProdutos.Core/*.csproj src/Soat.Eleven.GestaoProdutos.Core/
COPY src/Soat.Eleven.GestaoProdutos.Infra/*.csproj src/Soat.Eleven.GestaoProdutos.Infra/

RUN dotnet restore src/soat.eleven.pedido.sln

COPY . .

RUN dotnet publish "src/Soat.Eleven.GestaoProdutos.Api/Soat.Eleven.GestaoProdutos.Adapter.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS migrator

WORKDIR /app

COPY . . 

RUN dotnet tool install --global dotnet-ef --version 8.*

ENV PATH="/root/.dotnet/tools:${PATH}"

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final

WORKDIR /app

COPY --from=build-env /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "Soat.Eleven.GestaoProdutos.Adapter.WebApi.dll"]
