using Microsoft.AspNetCore.Authentication.Cookies;
using SolucoesDefeitos.BusinessDefinition;
using SolucoesDefeitos.Pesentation.RazorPages.Api;

namespace SolucoesDefeitos.Pesentation.RazorPages
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services
                .AddRazorPages()
                .AddRazorRuntimeCompilation()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/");
                    options.Conventions.AllowAnonymousToPage("/Identity/Account/Register");
                });

            builder.Services.ConfigureApplicationDependencies();

            builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                    options.SlidingExpiration = true;
                    options.LoginPath = "/Identity/Account/Login";
                    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                });

            Seed(builder);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();
            
            app.UseAuthorization();

            app.MapRazorPages();

            app.MapApiGroups();

            app.Run();
        }

        private static void Seed(WebApplicationBuilder builder)
        {
            var serviceProvider = builder.Services.BuildServiceProvider();
            var database = serviceProvider.GetRequiredService<IDatabase>();
            var seeders = serviceProvider.GetRequiredService<IEnumerable<ISeeder>>();
            foreach(var seeder in seeders)
            {
                seeder.Seed(database);
            }
        }
    }
}