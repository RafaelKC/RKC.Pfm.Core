using Microsoft.Extensions.Configuration;
using RKC.Pfm.Core.Infrastructure.Consts;

namespace RKC.Pfm.Core.Infrastructure.Database.Services;

public interface ISchemaNameProvider
{
    public string GetSchemaName();
}

public class SchemaNameProvider: ISchemaNameProvider
{
    private IConfiguration _configuration;

    public SchemaNameProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetSchemaName()
    {
        return _configuration[AppConfig.SchemaNameKey];
    }
}