using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using SistemaMatheus.Data;

// Carrega as variáveis de ambiente a partir do arquivo .env
Env.Load(@"../.env");

var builder = WebApplication.CreateBuilder(args);

// Adiciona suporte a controllers (necessário para API MVC)
builder.Services.AddControllers();

// Configuração de CORS para permitir acesso do front-end especificado
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://127.0.0.1:3000") // URL do front-end
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); 
        });
});

// Monta a connection string usando variáveis de ambiente
var connectionString =
    $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
    $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
    $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
    $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
    $"Password={Environment.GetEnvironmentVariable("DB_PASS")}";

// Configura o Entity Framework Core com PostgreSQL
builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuração do Swagger para documentação da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração extra para incluir comentários XML no Swagger (documentação detalhada)
builder.Services.AddSwaggerGen(c => 
{
    var xmlPath = Path.Combine(Directory.GetCurrentDirectory(), "SwaggerDocs/SeuProjeto.xml");
    c.IncludeXmlComments(xmlPath); // Habilita leitura de comentários XML
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Ativa CORS antes da autorização (importante para APIs acessadas externamente)
app.UseCors("AllowFrontend");

app.UseAuthorization();

// Mapeia os endpoints de controllers automaticamente
app.MapControllers();

app.Run();
