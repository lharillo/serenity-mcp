# syntax=docker/dockerfile:1.4
# Use .NET 10 SDK for building
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build

# Build arguments for versioning
ARG VERSION=1.0.0
ARG BUILD_DATE
ARG VCS_REF

WORKDIR /src

# Copy csproj and restore
COPY ["SerenityStarMcp.csproj", "./"]
RUN dotnet restore "SerenityStarMcp.csproj"

# Copy everything else and build
COPY . .
RUN dotnet publish "SerenityStarMcp.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS final
WORKDIR /app
COPY --from=build /app/publish .

# Create non-root user
RUN groupadd -r appuser && useradd -r -g appuser -u 1001 appuser
USER appuser

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV SerenityApi__BaseUrl=https://api.serenitystar.ai

# OCI Labels - https://github.com/opencontainers/image-spec/blob/main/annotations.md
LABEL org.opencontainers.image.title="Serenity Star MCP Server" \
      org.opencontainers.image.description="Model Context Protocol server for Serenity Star AI Platform with 30+ tools for AI agent management" \
      org.opencontainers.image.version="${VERSION:-1.0.0}" \
      org.opencontainers.image.created="${BUILD_DATE}" \
      org.opencontainers.image.revision="${VCS_REF}" \
      org.opencontainers.image.vendor="Subgen AI" \
      org.opencontainers.image.authors="Subgen AI <info@subgen.ai>" \
      org.opencontainers.image.url="https://mcp.starkcloud.cc/serenitystar" \
      org.opencontainers.image.documentation="https://mcp.starkcloud.cc/serenitystar" \
      org.opencontainers.image.source="https://github.com/subgenai/serenity-mcp" \
      org.opencontainers.image.licenses="MIT"

# Expose port
EXPOSE 8080

# Run the application
ENTRYPOINT ["dotnet", "SerenityStarMcp.dll"]