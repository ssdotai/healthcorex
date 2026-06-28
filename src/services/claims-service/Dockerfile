ARG REGISTRY=choacrhy6h2vdulfru6.azurecr.io
FROM ${REGISTRY}/dotnet/sdk:8.0 AS build
WORKDIR /repo

# Copy csproj files and restore (preserves layer caching).
# Directory.Build.props pins minimum versions for CVE-2024-43485
# (System.Formats.Asn1 RCE) and CVE-2024-21907 (Newtonsoft.Json).
# MSBuild auto-imports it by walking up from the csproj, so it must
# exist at /repo/src/ before `dotnet restore` runs — otherwise the
# resolved graph silently uses older, vulnerable package versions.
COPY src/Directory.Build.props src/
COPY src/services/claims-service/claims-service.csproj src/services/claims-service/
COPY src/services/shared/HealthCoreX.Infrastructure/HealthCoreX.Infrastructure.csproj src/services/shared/HealthCoreX.Infrastructure/
COPY src/services/shared/HealthCoreX.Events/HealthCoreX.Events.csproj src/services/shared/HealthCoreX.Events/
# 5.5 — BenefitCalculationStage calls IBenefitCalculationEngine via the
# HttpBenefitCalculationEngineClient shim, which transitively pulls in
# BenefitEngine + OperatingMode at compile time.
COPY src/engines/HealthCoreX.BenefitEngine/HealthCoreX.BenefitEngine.csproj src/engines/HealthCoreX.BenefitEngine/
COPY src/engines/HealthCoreX.OperatingMode/HealthCoreX.OperatingMode.csproj src/engines/HealthCoreX.OperatingMode/
# 5.4 — ScrubbingStage drives every claim through the shared scrub
# engine class library at adjudication pipeline Order=100.
COPY src/engines/HealthCoreX.ClaimsScrubEngine/HealthCoreX.ClaimsScrubEngine.csproj src/engines/HealthCoreX.ClaimsScrubEngine/
# 5.7 — NcciEditsStage drives every claim through the NCCI engine
# class library at adjudication pipeline Order=400.
COPY src/engines/HealthCoreX.NcciEngine/HealthCoreX.NcciEngine.csproj src/engines/HealthCoreX.NcciEngine/
# 5.8 — CoordinationOfBenefitsStage uses the CobEngine class library
# for payer-order rule labelling at adjudication pipeline Order=500.
COPY src/engines/HealthCoreX.CobEngine/HealthCoreX.CobEngine.csproj src/engines/HealthCoreX.CobEngine/
RUN dotnet restore src/services/claims-service/claims-service.csproj

# Copy source code and build
COPY src/services/claims-service/ src/services/claims-service/
COPY src/services/shared/HealthCoreX.Infrastructure/ src/services/shared/HealthCoreX.Infrastructure/
COPY src/services/shared/HealthCoreX.Events/ src/services/shared/HealthCoreX.Events/
COPY src/engines/HealthCoreX.BenefitEngine/ src/engines/HealthCoreX.BenefitEngine/
COPY src/engines/HealthCoreX.OperatingMode/ src/engines/HealthCoreX.OperatingMode/
COPY src/engines/HealthCoreX.ClaimsScrubEngine/ src/engines/HealthCoreX.ClaimsScrubEngine/
COPY src/engines/HealthCoreX.NcciEngine/ src/engines/HealthCoreX.NcciEngine/
COPY src/engines/HealthCoreX.CobEngine/ src/engines/HealthCoreX.CobEngine/
RUN dotnet build src/services/claims-service/claims-service.csproj -c Release --no-restore

# Publish stage
FROM build AS publish
RUN dotnet publish src/services/claims-service/claims-service.csproj -c Release -o /app/publish /p:UseAppHost=false --no-restore --no-build

# Runtime stage
FROM ${REGISTRY}/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

COPY --from=publish /app/publish .

RUN groupadd -r appuser && useradd -r -g appuser appuser \
    && chown -R appuser:appuser /app
USER appuser

EXPOSE 8080

HEALTHCHECK --interval=30s --timeout=3s --start-period=10s --retries=3 \
    CMD curl -f http://localhost:8080/health/live || exit 1

ENTRYPOINT ["dotnet", "claims-service.dll"]
