FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /app

COPY src/*.sln src/
COPY src/Soat.Eleven.FastFood.GestaoProdutos.Tests/*.csproj src/Soat.Eleven.FastFoodGestaoProdutos.Tests/
COPY src/Soat.Eleven.FastFood.GestaoProdutos.Adapter.WebApi/*.csproj src/Soat.Eleven.FastFood.GestaoProdutos.Adapter.WebApi/
COPY src/Soat.Eleven.FastFood.GestaoProdutos.Application/*.csproj src/Soat.Eleven.FastFood.GestaoProdutos.Application/
COPY src/Soat.Eleven.FastFood.GestaoProdutos.Core/*.csproj src/Soat.Eleven.FastFood.GestaoProdutos.Core/
COPY src/Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra/*.csproj src/Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra/

RUN dotnet restore src/soat.eleven.pedido.sln

COPY . .

RUN dotnet publish "src/Soat.Eleven.FastFood.GestaoProdutos.Adapter.WebApi/Soat.Eleven.FastFoodGestaoProdutos.Adapter.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS migrator

WORKDIR /app

COPY . . 

RUN dotnet tool install --global dotnet-ef --version 8.*

ENV PATH="/root/.dotnet/tools:${PATH}"

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final

WORKDIR /app

COPY --from=build-env /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "Soat.Eleven.FastFood.GestaoProdutos.Adapter.WebApi.dll"]
