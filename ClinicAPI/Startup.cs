using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PatientManager;

namespace ClinicAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configuración de Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ClinicAPI", Version = "v1" });
            });

            // Configuración de la aplicación
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // Otros servicios
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configuración para Swagger
            var appName = Configuration.GetSection("AppSettings:AppName").Value;
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{appName} v1");
            });

            // Otros middlewares
        }
    }
}
