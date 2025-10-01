# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copiar archivos de proyecto y restaurar dependencias
COPY JKC.Backend.Dominio.Entidades/JKC.Backend.Dominio.Entidades.csproj JKC.Backend.Dominio.Entidades/
COPY JKC.Backend.Dominio/JKC.Backend.Dominio.csproj JKC.Backend.Dominio/
COPY JKC.Backend.Infraestructura.Data/JKC.Backend.Infraestructura.Data.csproj JKC.Backend.Infraestructura.Data/
COPY JKC.Backend.Infraestructura.Framework/JKC.Backend.Infraestructura.Framework.csproj JKC.Backend.Infraestructura.Framework/
COPY JKC.Backend.Aplicacion/JKC.Backend.Aplicacion.csproj JKC.Backend.Aplicacion/
COPY JKC.Backend.Presentacion/JKC.Backend.WebApi.csproj JKC.Backend.Presentacion/

# Restaurar dependencias
RUN dotnet restore JKC.Backend.Presentacion/JKC.Backend.WebApi.csproj

# Copiar todo el código
COPY . .

# Restaurar dependencias usando solo fuentes en línea
RUN dotnet restore JKC.Backend.Presentacion/JKC.Backend.WebApi.csproj --no-cache

# Publicar el proyecto en modo Release
RUN dotnet publish JKC.Backend.Presentacion/JKC.Backend.WebApi.csproj -c Release -o /app/publish --no-restore
    #-c Release \
    #-o /app/publish \
    #--no-restore

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app



COPY --from=build /app/publish .

# Puerto que expondrá la API
EXPOSE 8080

# Comando para correr la aplicación
ENTRYPOINT ["dotnet", "JKC.Backend.WebApi.dll"]
