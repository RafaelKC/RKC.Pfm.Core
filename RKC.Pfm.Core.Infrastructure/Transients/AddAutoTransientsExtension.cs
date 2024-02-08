using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace RKC.Pfm.Core.Infrastructure.Transients;

public static class  AddAutoTransientsExtension
{
    public static IServiceCollection AddAutoTransients(this IServiceCollection services)
    {
        var serviceTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(type => type.GetInterfaces().Contains(typeof(IAutoTransient)));
        
        foreach (var serviceType in serviceTypes)
        {
            var normalInterface = serviceType.GetInterfaces().First(type => type != typeof(IAutoTransient));
            services.AddTransient(normalInterface, serviceType);
        }

        return services;
    }
}