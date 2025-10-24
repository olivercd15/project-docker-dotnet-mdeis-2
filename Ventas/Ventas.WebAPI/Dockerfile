FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

# Copiamos ambos proyectos desde el contexto
COPY Ventas.WebAPI/ ./Ventas.WebAPI/
COPY Ventas.Application/ ./Ventas.Application/
COPY Ventas.Infrastructure/ ./Ventas.Infrastructure/
COPY Ventas.Domain/ ./Ventas.Domain/

RUN dotnet restore ./Ventas.WebAPI/Ventas.WebAPI.csproj
RUN dotnet publish ./Ventas.WebAPI/Ventas.WebAPI.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Ventas.WebAPI.dll"]
