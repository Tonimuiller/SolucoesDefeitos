using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using System.IO;

namespace SolucoesDefeitos
{
    public class Startup
    {
        private const string SwaggerApiVersion = "v1";
        private const string SwaggerApiTitle = "Soluções e Defeitos Api";
        private const string SwaggerApiRouteName = "Soluções e Defeitos Api v1";
        private const string SwaggerApiRoute = "v1/swagger.json";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(options => 
            {
                options.SwaggerDoc(SwaggerApiVersion, new OpenApiInfo { Title = SwaggerApiTitle, Version = SwaggerApiVersion });
                string appPath =
                    PlatformServices.Default.Application.ApplicationBasePath;
                string appName =
                    PlatformServices.Default.Application.ApplicationName;
                string caminhoXmlDoc =
                    Path.Combine(appPath, $"{appName}.xml");

                options.IncludeXmlComments(caminhoXmlDoc);
            });
            services.ConfigureApplicationDependencies();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors((cors) => cors.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader());

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(options => 
            {
                options.SwaggerEndpoint(SwaggerApiRoute, SwaggerApiRouteName);
            });
        }
    }
}
