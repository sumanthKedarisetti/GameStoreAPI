# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project file and restore first to improve Docker layer caching
COPY ["GameStore.API.csproj", "./"]
RUN dotnet restore "GameStore.API.csproj"

# Copy source and publish
COPY . .
RUN dotnet publish "GameStore.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Make the app listen on port 5005 inside the container
ENV ASPNETCORE_HTTP_PORTS=5005
EXPOSE 5005
ENTRYPOINT ["dotnet", "GameStore.API.dll"]