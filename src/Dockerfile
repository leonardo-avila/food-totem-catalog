FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["Adapters/Driver/FoodTotem.Catalog.API/FoodTotem.Catalog.API.csproj", "Adapters/Driver/FoodTotem.Catalog.API/"]
COPY ["Core/FoodTotem.Catalog.UseCase/FoodTotem.Catalog.UseCase.csproj", "Core/FoodTotem.Catalog.UseCase/"]
COPY ["Core/FoodTotem.Catalog.Domain/FoodTotem.Catalog.Domain.csproj", "Core/FoodTotem.Catalog.Domain/"]
COPY ["Core/FoodTotem.Domain.Core/FoodTotem.Domain.Core.csproj", "Core/FoodTotem.Domain.Core/"]
COPY ["Core/FoodTotem.Data.Core/FoodTotem.Data.Core.csproj", "Core/FoodTotem.Data.Core/"]
COPY ["Adapters/Driven/FoodTotem.Catalog.Gateways.MySQL/FoodTotem.Catalog.Gateways.MySQL.csproj", "Adapters/Driven/FoodTotem.Catalog.Gateways.MySQL/"]

RUN dotnet restore "Adapters/Driver/FoodTotem.Catalog.API/FoodTotem.Catalog.API.csproj"
COPY . .
WORKDIR "/src/Adapters/Driver/FoodTotem.Catalog.API"
RUN dotnet build "FoodTotem.Catalog.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FoodTotem.Catalog.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FoodTotem.Catalog.API.dll"]