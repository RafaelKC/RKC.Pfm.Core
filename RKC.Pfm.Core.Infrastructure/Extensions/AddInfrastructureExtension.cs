using System.Text.Json;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RKC.Pfm.Core.Infrastructure.Authentication;
using RKC.Pfm.Core.Infrastructure.Consts;
using RKC.Pfm.Core.Infrastructure.Database;

namespace RKC.Pfm.Core.Infrastructure.Extensions;

public static class AddInfrastructureExtension
{
        public static IConfiguration AddInfrastructure(this IServiceCollection services)
        {
                var configuration = services.AddConsul();

                services
                        .AddDbContext<RkcPfmCoreDbContext>(op =>
                        {
                                op.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                        })
                        .AddScoped<IAuthenticationService, AuthenticationService>()
                        .AddHttpClient<IJwtProvider, JwtProvider>(client =>
                        {
                                client.BaseAddress = new Uri(configuration[AppConfig.AuthenticationTokenUriKey]);
                        });
                
                ConfigFirebase(configuration);

                return configuration;
        }

        private static void ConfigFirebase(IConfiguration configuration)
        {
                var googleCredentials = configuration.GetSection(AppConfig.FirebaseCredentialsKey).Get<GoogleCredentialDto>();
                
                FirebaseApp.Create(new AppOptions
                {
                        Credential = GoogleCredential.FromJson(JsonSerializer.Serialize(googleCredentials))
                });
        }
        
        private class GoogleCredentialDto
        {
                public string type { get; set; }
                public string project_id { get; set; }
                public string private_key_id { get; set; }
                public string private_key { get; set; }
                public string client_email { get; set; }
                public string client_id { get; set; }
                public string auth_uri { get; set; }
                public string token_uri { get; set; }
                public string auth_provider_x509_cert_url { get; set; }
                public string client_x509_cert_url { get; set; }
        }
}