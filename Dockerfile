FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY *.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Set environment variables for MCP protocol compliance
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_LOGGING_CONSOLE_DISABLECOLORS=true

ENTRYPOINT ["dotnet", "SerenityStarMcp.dll"]