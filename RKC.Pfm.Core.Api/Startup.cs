﻿using RKC.Pfm.Core.Application.Transients;
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