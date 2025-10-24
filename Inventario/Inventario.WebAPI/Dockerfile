FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

# Copiamos ambos proyectos desde el contexto
COPY Inventario.WebAPI/ ./Inventario.WebAPI/
COPY Inventario.Application/ ./Inventario.Application/
COPY Inventario.Infrastructure/ ./Inventario.Infrastructure/
COPY Inventario.Domain/ ./Inventario.Domain/

RUN dotnet restore ./Inventario.WebAPI/Inventario.WebAPI.csproj
RUN dotnet publish ./Inventario.WebAPI/Inventario.WebAPI.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Inventario.WebAPI.dll"]
