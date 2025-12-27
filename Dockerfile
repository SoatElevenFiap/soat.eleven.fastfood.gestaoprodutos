FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /app

COPY src/*.sln src/
COPY src/Soat.Eleven.FastFoodGestaoProdutos.Tests/*.csproj src/Soat.Eleven.FastFoodGestaoProdutos.Tests/
COPY src/Soat.Eleven.FastFoodGestaoProdutos.Api/*.csproj src/Soat.Eleven.FastFoodGestaoProdutos.WebApi/
COPY src/Soat.Eleven.FastFoodGestaoProdutos.Application/*.csproj src/Soat.Eleven.FastFoodGestaoProdutos.Application/
COPY src/Soat.Eleven.FastFoodGestaoProdutos.Core/*.csproj src/Soat.Eleven.FastFoodGestaoProdutos.Core/
COPY src/Soat.Eleven.FastFoodGestaoProdutos.Infra/*.csproj src/Soat.Eleven.FastFoodGestaoProdutos.Infra/

RUN dotnet restore src/soat.eleven.pedido.sln

COPY . .

RUN dotnet publish "src/Soat.Eleven.FastFoodGestaoProdutos.Api/Soat.Eleven.FastFoodGestaoProdutos.Adapter.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS migrator

WORKDIR /app

COPY . . 

RUN dotnet tool install --global dotnet-ef --version 8.*

ENV PATH="/root/.dotnet/tools:${PATH}"

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final

WORKDIR /app

COPY --from=build-env /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "Soat.Eleven.FastFoodGestaoProdutos.Adapter.WebApi.dll"]
