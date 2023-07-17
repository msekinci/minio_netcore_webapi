FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["minio_netcore_webapi/minio_netcore_webapi.csproj", "minio_netcore_webapi/"]
RUN dotnet restore "minio_netcore_webapi/minio_netcore_webapi.csproj"
COPY . .
WORKDIR "/src/minio_netcore_webapi"
RUN dotnet build "minio_netcore_webapi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "minio_netcore_webapi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Minio'yu ekleyelim
RUN apt-get update && apt-get install -y wget
RUN wget https://dl.min.io/server/minio/release/linux-amd64/minio
RUN chmod +x minio

ENTRYPOINT ["dotnet", "minio_netcore_webapi.dll"]
