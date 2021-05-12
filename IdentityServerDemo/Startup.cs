using IdentityServer.Helpers;
using IdentityServer4.Services;
using IdentityServerDemo.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace IdentityServerDemo
{
    public class Startup
    {
        private readonly MyConfiguration _myConfiguration;

        private IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;

            _myConfiguration = Configuration.GetSection("MyConfiguration").Get<MyConfiguration>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Services
            // ITokenService for debugging tokens
            services.AddTransient<ITokenService, MyTokenService>();

            // IProfileService for adding/removing claims from a token
            services.AddTransient<IProfileService, MyProfileService>();

            IIdentityServerBuilder builder = AddMyIdentityServer(services);

            // Signing Key
            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                throw new Exception("Configure Signing Key");
            }

            // Authorization & Authentication
            services.AddAuthorization();
            services.AddAuthentication();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
        
        private static IIdentityServerBuilder AddMyIdentityServer(IServiceCollection services)
        {
            var builder = services.AddIdentityServer()
                            .AddInMemoryClients(Config.GetClients())
                            //.AddInMemoryIdentityResources()
                            .AddInMemoryApiResources(Config.GetApiResources())
                            .AddInMemoryApiScopes(Config.GetApiScopes())
                            //.AddTestUsers()
                            .AddProfileService<MyProfileService>();
            return builder;
        }
    }
}
