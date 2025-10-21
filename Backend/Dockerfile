# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["AutomationSystem.API/AutomationSystem.API.csproj", "AutomationSystem.API/"]
COPY ["AutomationSystem.Core/AutomationSystem.Core.csproj", "AutomationSystem.Core/"]
COPY ["AutomationSystem.Infrastructure/AutomationSystem.Infrastructure.csproj", "AutomationSystem.Infrastructure/"]
RUN dotnet restore "AutomationSystem.API/AutomationSystem.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/AutomationSystem.API"
RUN dotnet build "AutomationSystem.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "AutomationSystem.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Install dependencies for PuppeteerSharp (if using)
RUN apt-get update && apt-get install -y \
    wget \
    gnupg \
    ca-certificates \
    && rm -rf /var/lib/apt/lists/*

COPY --from=publish /app/publish .

# Create directories for uploads and logs
RUN mkdir -p /app/uploads /app/logs

ENTRYPOINT ["dotnet", "AutomationSystem.API.dll"]

