using POC_CRUD.Configurations;
using POC_CRUD.Repositories;
using POC_CRUD.Services;
using ApiVersion = Microsoft.AspNetCore.Mvc.ApiVersion;

var builder = WebApplication.CreateBuilder(args);

// Obtêm os dados da aplicação. Obs: se não for encontrado, a aplicação falha ao subir (não opcional).
// Isso não é obirgatório, essa implementação é minha mesmo
builder.Configuration.AddJsonFile("version.json", optional: false, reloadOnChange: true).Build();

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

// Adiciona a configuração do MySQL a partir do appsettings.json
builder.Services.AddMySqlConfiguration(builder.Configuration);

// Injeção de dependências (para repositórios e services)
builder.Services.AddRepositories();
builder.Services.AddServices();

var app = builder.Build();

app.MapControllers();

app.Run();