using Microsoft.EntityFrameworkCore;
using RKC.Pfm.Core.Infrastructure;
using RKC.Pfm.Core.Infrastructure.Database;
using RKC.Pfm.Core.Infrastructure.Extensions;
using RKC.Pfm.Core.Infrastructure.Transients;

namespace RKC.Pfm.Core.Api;

public class Startup
{
    public IConfiguration Configuration { get; }   
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    
     public void ConfigureServices(IServiceCollection services)
        {

            services
                .AddSingleton(Configuration)
                .AddInfrastructure(Configuration)
                .AddCors()
                .AddSwaggerGen()
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

            app.UseRouting();

            app.UseCors(e => e
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
}