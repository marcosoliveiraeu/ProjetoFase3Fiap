using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pagamentos.Application.Services;
using Pagamentos.Infrastructure.Data;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        // A connection string vem das configurações da Function App (no Azure)
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__PagamentosDb");

        // Registro do DbContext apontando para SQL Server
        services.AddDbContext<PagamentosDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Registro do serviço que processa o pagamento
        services.AddScoped<GatewayPagamentoService>();
    })
    .Build();

host.Run();
