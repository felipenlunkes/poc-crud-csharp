using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Converters;
using POC_CRUD.Configurations;
using POC_CRUD.Exceptions;
using ApiVersion = Microsoft.AspNetCore.Mvc.ApiVersion;

var builder = WebApplication.CreateBuilder(args);

// Obtêm os dados da aplicação. Obs: se não for encontrado, a aplicação falha ao subir (não opcional).
// Isso não é obrigatório, essa implementação é minha mesmo (pode-se mover as configs para o arquivo
// appsettings.json)
builder.Configuration.AddJsonFile("version.json", optional: false, reloadOnChange: true).Build();

// serde JSON C#
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.Converters.Add(new StringEnumConverter()); // Serializa todos os enuns para string
    });

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true; // Se não especificar a versão, usa a padrão
    options.DefaultApiVersion = new ApiVersion(1, 0); // Versão v1
    options.ReportApiVersions = true; // Mostra no header da resposta as versões disponíveis
});

// Vamos configurar o processo de autenticação da aplicação
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

// Adiciona a configuração do MySQL à partir do appsettings.json
builder.Services.AddMySqlConfiguration(builder.Configuration);

// Injeção de dependências (para repositories e services)
builder.Services.AddRepositories();
builder.Services.AddServices();

// Configuração CORS para permitir requisições de origem cruzada do frontend Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Configurações de envio de email
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Consolidar o contexto da aplicação
var app = builder.Build();

// Rodar as migrations de forma automática na base
MySqlConfig.ApplyMigrations(app.Services);

// Mapear os controllers
app.MapControllers();

// Configura autenticação
app.UseAuthentication();

// Irá utilizar autenticação em endpoints protegidos
app.UseAuthorization();

// Incluir handlers de exceções: mapeamento para resposta HTTP
app.UseMiddleware<ExceptionHandler>();

app.UseCors("AngularPolicy");

app.Run();