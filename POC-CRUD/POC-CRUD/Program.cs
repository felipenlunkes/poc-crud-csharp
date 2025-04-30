using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using POC_CRUD.Configurations;
using POC_CRUD.Exceptions;
using ApiVersion = Microsoft.AspNetCore.Mvc.ApiVersion;

var builder = WebApplication.CreateBuilder(args);

// Obtêm os dados da aplicação. Obs: se não for encontrado, a aplicação falha ao subir (não opcional).
// Isso não é obirgatório, essa implementação é minha mesmo
builder.Configuration.AddJsonFile("version.json", optional: false, reloadOnChange: true).Build();

// serde JSON C#
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true; // Se não especificar a versão, usa a padrão
    options.DefaultApiVersion = new ApiVersion(1, 0); // Versão v1
    options.ReportApiVersions = true; // Mostra no header da resposta as versões disponíveis
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            )
        };
    });

builder.Services.AddAuthorization();

// Adiciona a configuração do MySQL a partir do appsettings.json
builder.Services.AddMySqlConfiguration(builder.Configuration);

// Injeção de dependências (para repositórios e services)
builder.Services.AddRepositories();
builder.Services.AddServices();

var app = builder.Build();

// Rodar as migrations de forma automática na base
MySqlConfig.ApplyMigrations(app.Services);

app.MapControllers();

// Configura autenticação
app.UseAuthentication();

// Irá utilizar autenticação em endpoints protegidos
app.UseAuthorization();

app.UseMiddleware<ExceptionHandler>();

app.Run();