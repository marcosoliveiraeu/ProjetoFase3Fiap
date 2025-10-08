# Imagem base para build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia os arquivos da solução
COPY . .

# Restaura as dependências
RUN dotnet restore Usuarios.csproj

# Build da aplicação
RUN dotnet publish Usuarios.csproj -c Release -o /app/publish

# Imagem base para runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copia os arquivos publicados do build
COPY --from=build /app/publish .

# Expõe a porta padrão do ASP.NET
EXPOSE 80

# Inicia a aplicação
ENTRYPOINT ["dotnet", "Usuarios.dll"]