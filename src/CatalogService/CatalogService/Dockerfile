# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY CatalogService.csproj .
RUN dotnet restore CatalogService.csproj

# Copy the rest of the source
COPY . .
RUN dotnet publish CatalogService.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Copy wait-for-it script and set execute permission
COPY wait-for-it.sh /app/wait-for-it.sh
RUN chmod +x /app/wait-for-it.sh

# Wait for DB and RabbitMQ, then run service
ENTRYPOINT ["sh", "-c", "./wait-for-it.sh catalog_db:3306 -- ./wait-for-it.sh rabbitmq:5672 -- dotnet CatalogService.dll"]
