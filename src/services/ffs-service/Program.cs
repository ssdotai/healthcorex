using HealthCoreX.Infrastructure.Configuration;
using HealthCoreX.Infrastructure.Json;
using HealthCoreX.Infrastructure.Observability;

var builder = WebApplication.CreateBuilder(args);
// Secret provider (Azure Key Vault / none)
builder.Services.AddSecretProvider(builder.Configuration);
builder.Configuration.AddAzureKeyVaultConfiguration(builder.Configuration);
builder.Services.AddControllers()
    .AddHealthCoreXJsonOptions();
builder.Services.AddChoObservability(builder.Configuration);
var app = builder.Build();
app.UseChoObservability();
app.MapControllers();
app.Run();

// Required for WebApplicationFactory<Program> in integration tests
public partial class Program { }
