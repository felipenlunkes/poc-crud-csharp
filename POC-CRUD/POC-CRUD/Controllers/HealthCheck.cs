using Microsoft.AspNetCore.Mvc;

namespace POC_CRUD.Controllers;

[Route("rest/v{version:apiVersion}/health")]
[ApiVersion("1")] // Define a versão da API
public class HealthCheckController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public HealthCheckController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult GetHealthStatus()
    {
        var name = _configuration["serviceName"] ?? "unknown";
        var version = _configuration["version"] ?? "unknown";
        var build = _configuration["build"] ?? "unknown";
        var releaseDate = _configuration["releaseDate"] ?? "unknown";

        var healthStatus = new
        {
            name,
            version,
            build,
            releaseDate,
            status = "Healthy"
        };

        return Ok(healthStatus);
    }
}