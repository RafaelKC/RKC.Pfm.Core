using System.Net;
using Microsoft.Net.Http.Headers;
using RKC.Pfm.Core.Application.Periods;
using RKC.Pfm.Core.Application.Transients;
using RKC.Pfm.Core.Infrastructure.Authentication;
using RKC.Pfm.Core.Infrastructure.Extensions;

namespace RKC.Pfm.Core.Api;

public class Startup
{
    
     public void ConfigureServices(IServiceCollection services)
     {
         var configurations = services.AddInfrastructure();
         
            services
                .AddSwaggerGen()
                .AddAutoTransients()
                .AddCors()
                .ConfigureHttpJsonOptions(op =>
                {
                    op.SerializerOptions.PropertyNameCaseInsensitive = true;
                })
                .AddHostedService<PeriodsBackgroundJob>()
                .AddControllers();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(e => e
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<LoggedOutUserMiddleware>();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
}