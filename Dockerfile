# 1. Imagen del SDK para compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos la solución y los archivos de proyecto para restaurar dependencias
# Esto es vital por la estructura de capas que tenés (Core, Infrastructure, etc.)
COPY ["TeraGestion.sln", "./"]
COPY ["Controllers/Controllers.csproj", "Controllers/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["Infraestructure/Infrastructure.csproj", "Infraestructure/"]
COPY ["Services/Services.csproj", "Services/"]
COPY ["TeraGestion.Tests/TeraGestion.Tests.csproj", "TeraGestion.Tests/"]

# Restauramos los paquetes NuGet
RUN dotnet restore

# Copiamos el resto del código
COPY . .

# Publicamos la aplicación
WORKDIR "/src/Controllers"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# 2. Imagen de runtime (más liviana)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Puerto por defecto de .NET 8
EXPOSE 8080
ENTRYPOINT ["dotnet", "Controllers.dll"]