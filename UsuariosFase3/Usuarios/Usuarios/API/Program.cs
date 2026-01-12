using Microsoft.EntityFrameworkCore;
using Usuarios.Application.Services;
using Usuarios.Application.Services.Interfaces;
using Usuarios.Infrastructure.Data;
using Usuarios.Infrastructure.Repository;
using Usuarios.Infrastructure.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var envFull = Environment.GetEnvironmentVariable("DB_CONNECTIONSTRING");
string connectionString;

if (!string.IsNullOrWhiteSpace(envFull))
{
    connectionString = envFull;
}
else
{
    var server = Environment.GetEnvironmentVariable("DB_SERVER");
    if (!string.IsNullOrWhiteSpace(server))
    {
        var database = Environment.GetEnvironmentVariable("DB_NAME") ?? string.Empty;
        var user = Environment.GetEnvironmentVariable("DB_USER") ?? string.Empty;
        var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? string.Empty;
        var encrypt = Environment.GetEnvironmentVariable("DB_ENCRYPT") ?? "True";
        var trustCert = Environment.GetEnvironmentVariable("DB_TRUST_CERT") ?? "False";
        var timeout = Environment.GetEnvironmentVariable("DB_TIMEOUT") ?? "30";

        connectionString =
            $"Server={server};" +
            $"Initial Catalog={database};" +
            $"User ID={user};" +
            $"Password={password};" +
            $"Encrypt={encrypt};" +
            $"TrustServerCertificate={trustCert};" +
            $"Connection Timeout={timeout};";
    }
    else
    {
        connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    }
}


builder.Services.AddDbContext<DbContextFCG>(options =>
        options.UseSqlServer(connectionString));

builder.Services.AddScoped<ISenhaHasher, SenhaHasher>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

var pathBase = Environment.GetEnvironmentVariable("ASPNETCORE_PATHBASE");
if (!string.IsNullOrEmpty(pathBase))
{
    app.UsePathBase(pathBase);
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger(c =>
{
    c.PreSerializeFilters.Add((swagger, httpReq) =>
    {
        swagger.Servers = new List<Microsoft.OpenApi.Models.OpenApiServer>
        {
            new Microsoft.OpenApi.Models.OpenApiServer
            {
                Url = $"{httpReq.Scheme}://{httpReq.Host.Value}{pathBase}"
            }
        };
    });
});

app.UseSwaggerUI(c => 
{
    c.SwaggerEndpoint($"{pathBase}/swagger/v1/swagger.json", "Usuarios API v1"); 
    c.RoutePrefix = "swagger"; 
});



//}

app.UseRouting();

//app.UseHttpsRedirection();

app.UseCors("CorsPolicy");


app.UseAuthorization();

app.MapControllers();

app.Run();
