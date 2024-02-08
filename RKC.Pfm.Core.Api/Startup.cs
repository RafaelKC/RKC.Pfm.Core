using Microsoft.EntityFrameworkCore;
using RKC.Pfm.Core.Application.Transients;
using RKC.Pfm.Core.Infrastructure;

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
                .AddDbContext<RkcPfmCoreDbContext>(op =>
                {
                    op.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
                })
                .AddCors()
                .AddAutoTransients()
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