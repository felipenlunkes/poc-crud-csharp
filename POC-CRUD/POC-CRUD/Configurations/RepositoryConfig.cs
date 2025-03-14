using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using POC_CRUD.Repositories;

namespace POC_CRUD.Configurations;

public static class RepositoryConfig
{
    public static void AddRepositories(this IServiceCollection services)
    {
        var repositoryInterface = typeof(IRepository);

        // Busca todas as classes que implementam a interface IRepository
        var types = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && repositoryInterface.IsAssignableFrom(t));

        // Adiciona cada repositório como Scoped
        foreach (var type in types)
        {
            services.AddScoped(type);
        }
    }
}
