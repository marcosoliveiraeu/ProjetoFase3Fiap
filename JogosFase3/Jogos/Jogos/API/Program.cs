using Jogos.Application.Services;
using Jogos.Application.Services.Interfaces;
using Jogos.Infrastructure.Data;
using Jogos.Infrastructure.Repository;
using Jogos.Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContextFCG>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IJogoService, JogoService>();
builder.Services.AddScoped<IJogosUsuarioService, JogosUsuarioService>();

builder.Services.AddScoped<IJogoRepository, JogoRepository>();
builder.Services.AddScoped<IJogosUsuarioRepository, JogosUsuarioRepository>();

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

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
