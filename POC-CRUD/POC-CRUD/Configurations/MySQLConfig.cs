using POC_CRUD.Data;

namespace POC_CRUD.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

public static class MySqlConfig
{
    public static void AddMySqlConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MySqlConnection");
        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 32))));
    }
}
