FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy only the .csproj and restore (no .sln needed)
COPY SecureCommerce_api/*.csproj ./SecureCommerce_api/
RUN dotnet restore ./SecureCommerce_api/SecureCommerce_api.csproj

# Copy everything else and publish
COPY . ./
RUN dotnet publish ./SecureCommerce_api/SecureCommerce_api.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

EXPOSE 8080
ENTRYPOINT ["dotnet", "SecureCommerce_api.dll"]