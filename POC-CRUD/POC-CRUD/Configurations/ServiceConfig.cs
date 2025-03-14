using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using POC_CRUD.Services;

namespace POC_CRUD.Configurations;

public static class ServiceConfig
{
    public static void AddServices(this IServiceCollection services)
    {
        var serviceInterface = typeof(IService);

        // Busca todas as classes que implementam a interface IService
        var types = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && serviceInterface.IsAssignableFrom(t));

        // Adiciona cada service como Scoped
        foreach (var type in types)
        {
            services.AddScoped(type);
        }
    }
}
