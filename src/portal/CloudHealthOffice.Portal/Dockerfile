ARG REGISTRY=choacrhy6h2vdulfru6.azurecr.io
# Stage 1: Build
FROM ${REGISTRY}/dotnet/sdk:8.0 AS build
WORKDIR /repo

COPY src/portal/HealthCoreX.Portal/HealthCoreX.Portal.csproj src/portal/HealthCoreX.Portal/
COPY src/services/shared/HealthCoreX.Infrastructure/HealthCoreX.Infrastructure.csproj src/services/shared/HealthCoreX.Infrastructure/
RUN dotnet restore src/portal/HealthCoreX.Portal/HealthCoreX.Portal.csproj

COPY src/portal/HealthCoreX.Portal/ src/portal/HealthCoreX.Portal/
COPY src/services/shared/HealthCoreX.Infrastructure/ src/services/shared/HealthCoreX.Infrastructure/
RUN dotnet build src/portal/HealthCoreX.Portal/HealthCoreX.Portal.csproj -c Release --no-restore

# Publish stage
FROM build AS publish
RUN dotnet publish src/portal/HealthCoreX.Portal/HealthCoreX.Portal.csproj -c Release -o /app/publish /p:UseAppHost=false --no-restore --no-build

# Stage 2: Runtime
FROM ${REGISTRY}/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

RUN groupadd -r appuser && useradd -r -g appuser appuser
USER appuser

COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

HEALTHCHECK --interval=10s --timeout=5s --start-period=15s --retries=10 \
    CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "HealthCoreX.Portal.dll"]
