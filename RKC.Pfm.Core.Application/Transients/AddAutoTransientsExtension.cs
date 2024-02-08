using Microsoft.Extensions.DependencyInjection;

namespace RKC.Pfm.Core.Application.Transients;

public static class  AddAutoTransientsExtension
{
    public static IServiceCollection AddAutoTransients(this IServiceCollection services)
    {
        RegisterDependencyByType(services, typeof(IAutoTransient), ServiceLifetime.Transient);

        return services;
    }
    
    private static void RegisterDependencyByType(IServiceCollection serviceCollection, Type dependencyType, ServiceLifetime lifeStyle)
    {
        var dependencies =
            from dependency in AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
            where dependency.GetInterfaces().Contains(dependencyType)
            group dependency by dependency.GetDirectImplementedInterfaceFromType()
            into groupedDependencies
            select new
            {
                HasMoreThanOneClassImplementingService = groupedDependencies.Count() > 1,
                ServiceType = groupedDependencies.Key,
                ImplementationTypes = groupedDependencies
            };  

        foreach (var dependency in dependencies)
        {
            foreach (var implementationType in dependency.ImplementationTypes)
            {
                var serviceDescriptor = new ServiceDescriptor(dependency.ServiceType, implementationType, lifeStyle);
                serviceCollection.Add(serviceDescriptor);
            }
        }
    }
    
    private static Type GetDirectImplementedInterfaceFromType(this Type type)
    {
        return type.BaseType != null
            ? type.GetInterfaces().Except(type.BaseType.GetInterfaces()).First()
            : type.GetInterfaces().First();
    }
}