FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["StockService/StockService.csproj", "StockService/"]
RUN dotnet restore "StockService/StockService.csproj"
COPY . .
WORKDIR "/src/StockService"
RUN dotnet build "StockService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StockService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StockService.dll"]