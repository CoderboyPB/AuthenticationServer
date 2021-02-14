using AuthenticationServerAPI.Models;
using AuthenticationServerAPI.Services.Authenticators;
using AuthenticationServerAPI.Services.PasswordHashes;
using AuthenticationServerAPI.Services.RefreshTokenRepositories;
using AuthenticationServerAPI.Services.TokenGenerators;
using AuthenticationServerAPI.Services.TokenValidators;
using AuthenticationServerAPI.Services.UserRepositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServerAPI
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            AuthenticationConfiguration authenticationConfiguration = new AuthenticationConfiguration();
            _configuration.Bind("Authentication", authenticationConfiguration);
            services.AddSingleton(authenticationConfiguration);

            services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
            services.AddSingleton<IUserRepository, InMemoryUserRepository>();
            services.AddSingleton<IRefreshTokenRepository, InMemoryRefreshTokenRepository>();
            services.AddSingleton<Authenticator>();

            services.AddSingleton<TokenGenerator>();
            services.AddSingleton<AccessTokenGenerator>();
            services.AddSingleton<RefreshTokenGenerator>();

            services.AddSingleton<RefreshTokenValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
