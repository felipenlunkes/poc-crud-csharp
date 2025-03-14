using ApiVersion = Microsoft.AspNetCore.Mvc.ApiVersion;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("version.json", optional: false, reloadOnChange: true).Build();

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true; // Se não especificar a versão, usa a padrão
    options.DefaultApiVersion = new ApiVersion(1, 0); // Versão padrão v1.0
    options.ReportApiVersions = true; // Mostra no header da resposta as versões disponíveis
});

var app = builder.Build();

app.MapControllers();

app.Run();