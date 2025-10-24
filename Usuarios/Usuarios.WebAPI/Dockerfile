FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

# Copiamos ambos proyectos desde el contexto
COPY Usuarios.WebAPI/ ./Usuarios.WebAPI/
COPY Usuarios.Application/ ./Usuarios.Application/
COPY Usuarios.Infrastructure/ ./Usuarios.Infrastructure/
COPY Usuarios.Domain/ ./Usuarios.Domain/

RUN dotnet restore ./Usuarios.WebAPI/Usuarios.WebAPI.csproj
RUN dotnet publish ./Usuarios.WebAPI/Usuarios.WebAPI.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Usuarios.WebAPI.dll"]
