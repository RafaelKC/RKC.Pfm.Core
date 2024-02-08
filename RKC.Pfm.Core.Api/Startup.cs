using RKC.Pfm.Core.Infrastructure.Extensions;

namespace RKC.Pfm.Core.Api;

public class Startup
{
    
     public void ConfigureServices(IServiceCollection services)
     {
         var configurations = services.AddInfrastructure();
         
            services
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