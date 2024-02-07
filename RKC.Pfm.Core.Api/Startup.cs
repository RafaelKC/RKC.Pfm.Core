using RKC.Pfm.Core.Application.Transients;
using RKC.Pfm.Core.Infrastructure;
using RKC.Pfm.Core.Infrastructure.Services;

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
                .AddDbContext<RkcPfmCoreDbContext>()
                .AddCors()
                .AddAutoTransients()
                .AddScoped<MigrationService>()
                .AddControllers();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MigrationService migrationService)
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
            
            migrationService.ApplyMigrations();
        }
}