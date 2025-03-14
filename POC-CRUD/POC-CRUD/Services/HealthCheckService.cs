using POC_CRUD.Models;

namespace POC_CRUD.Services;

public class HealthCheckService : IService
{
    private readonly IConfiguration _configuration;
    
    public HealthCheckService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public HealthStatus getHealthStatus()
    {
        var name = _configuration["serviceName"] ?? "unknown";
        var version = _configuration["version"] ?? "unknown";
        var build = _configuration["build"] ?? "unknown";
        var releaseDate = _configuration["releaseDate"] ?? "unknown";
        var status = "Healthy";

        return new HealthStatus(name, version, build, releaseDate, status);
    }
    
}