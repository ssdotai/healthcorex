ARG REGISTRY=choacrhy6h2vdulfru6.azurecr.io
FROM ${REGISTRY}/dotnet/sdk:8.0 AS build
WORKDIR /repo

# Copy csproj files and restore (preserves layer caching)
COPY src/services/benefit-plan-service/benefit-plan-service.csproj src/services/benefit-plan-service/
COPY src/services/shared/HealthCoreX.Infrastructure/HealthCoreX.Infrastructure.csproj src/services/shared/HealthCoreX.Infrastructure/
COPY src/engines/HealthCoreX.BenefitEngine/HealthCoreX.BenefitEngine.csproj src/engines/HealthCoreX.BenefitEngine/
COPY src/engines/HealthCoreX.OperatingMode/HealthCoreX.OperatingMode.csproj src/engines/HealthCoreX.OperatingMode/
COPY src/engines/HealthCoreX.FeeScheduleEngine/HealthCoreX.FeeScheduleEngine.csproj src/engines/HealthCoreX.FeeScheduleEngine/
COPY src/engines/HealthCoreX.ClaimsScrubEngine/HealthCoreX.ClaimsScrubEngine.csproj src/engines/HealthCoreX.ClaimsScrubEngine/
COPY src/engines/HealthCoreX.NcciEngine/HealthCoreX.NcciEngine.csproj src/engines/HealthCoreX.NcciEngine/
COPY src/engines/HealthCoreX.ProviderEnrollmentService/HealthCoreX.ProviderEnrollmentService.csproj src/engines/HealthCoreX.ProviderEnrollmentService/
COPY src/engines/HealthCoreX.PriorAuthRuleEngine/HealthCoreX.PriorAuthRuleEngine.csproj src/engines/HealthCoreX.PriorAuthRuleEngine/
RUN dotnet restore src/services/benefit-plan-service/benefit-plan-service.csproj

# Copy source code and build
COPY src/services/benefit-plan-service/ src/services/benefit-plan-service/
COPY src/services/shared/HealthCoreX.Infrastructure/ src/services/shared/HealthCoreX.Infrastructure/
COPY src/engines/HealthCoreX.BenefitEngine/ src/engines/HealthCoreX.BenefitEngine/
COPY src/engines/HealthCoreX.OperatingMode/ src/engines/HealthCoreX.OperatingMode/
COPY src/engines/HealthCoreX.FeeScheduleEngine/ src/engines/HealthCoreX.FeeScheduleEngine/
COPY src/engines/HealthCoreX.ClaimsScrubEngine/ src/engines/HealthCoreX.ClaimsScrubEngine/
COPY src/engines/HealthCoreX.NcciEngine/ src/engines/HealthCoreX.NcciEngine/
COPY src/engines/HealthCoreX.ProviderEnrollmentService/ src/engines/HealthCoreX.ProviderEnrollmentService/
COPY src/engines/HealthCoreX.PriorAuthRuleEngine/ src/engines/HealthCoreX.PriorAuthRuleEngine/
# benefit-plan-service.csproj declares <Content Include="..\..\..\schemas\..."> for
# the SystemDefaultMappingSeeder seed bundle (BP 5.6) and the AcaLimitsProvider OOP
# limits file (BP 5.7); MSBuild copies them to the build output via
# CopyToOutputDirectory="PreserveNewest". Without these COPY lines the build fails
# with MSB3030 "Could not copy the file ... because it was not found."
COPY schemas/service-category-mappings/ schemas/service-category-mappings/
COPY schemas/aca-oop-limits/ schemas/aca-oop-limits/
RUN dotnet build src/services/benefit-plan-service/benefit-plan-service.csproj -c Release --no-restore

# Publish stage
FROM build AS publish
RUN dotnet publish src/services/benefit-plan-service/benefit-plan-service.csproj -c Release -o /app/publish /p:UseAppHost=false --no-restore --no-build

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

ENTRYPOINT ["dotnet", "benefit-plan-service.dll"]
