using Microsoft.EntityFrameworkCore;
using UniHub.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Adiciona o suporte aos Controllers (para ele reconhecer o AuthController)
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Configurações para o Swagger (interface gráfica para testar a API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 3. Habilita o Swagger no ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

// 4. Mapeia as rotas dos seus controladores
app.MapControllers();

app.Run();