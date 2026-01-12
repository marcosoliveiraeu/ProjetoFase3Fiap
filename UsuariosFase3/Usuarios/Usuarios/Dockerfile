# ---------- Build ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos da solução
COPY . .

# Restaura dependências
RUN dotnet restore Usuarios.csproj

# Publica a aplicação
RUN dotnet publish Usuarios.csproj -c Release -o /app/publish

# ---------- Runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copia os arquivos publicados
COPY --from=build /app/publish .

# Porta usada no Kubernetes
EXPOSE 8080

# Inicia a aplicação
ENTRYPOINT ["dotnet", "Usuarios.dll"]
