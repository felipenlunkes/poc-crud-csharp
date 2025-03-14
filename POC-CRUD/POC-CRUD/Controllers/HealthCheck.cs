using Microsoft.AspNetCore.Mvc;
using POC_CRUD.Services;

namespace POC_CRUD.Controllers;

[Route("rest/v{version:apiVersion}/health")]
[ApiVersion("1")] // Define a versão da API
public class HealthCheckController : ControllerBase
{

    private readonly HealthCheckService _healthCheckService;
    
    public HealthCheckController(HealthCheckService healthCheckService)
    {
        _healthCheckService = healthCheckService;
    }

    [HttpGet]
    public IActionResult GetHealthStatus()
    {
        var healthStatus = _healthCheckService.getHealthStatus();

        return Ok(healthStatus);
    }
}