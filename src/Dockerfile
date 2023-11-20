FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["Adapters/Driver/FoodTotem.Catalog.API/FoodTotem.Catalog.API.csproj", "Adapters/Driver/FoodTotem.Catalog.API/"]
# COPY ["Adapters/Driven/*/*.csproj", "Adapters/Driven/"]
# COPY ["Core/*/*.csproj", "Core/"]

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