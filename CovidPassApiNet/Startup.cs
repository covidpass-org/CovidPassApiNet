using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CovidPass_API.Options;
using CovidPass_API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace CovidPass_API
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
            // Add configurations from appsettings.json
            services.AddOptions<HashOptions>().Bind(Configuration.GetSection(HashOptions.Key))
                .ValidateDataAnnotations();
            services.AddOptions<CertificateOptions>().Bind(Configuration.GetSection(CertificateOptions.Key))
                .ValidateDataAnnotations();
            services.AddOptions<ServerOptions>().Bind(Configuration.GetSection(ServerOptions.Key))
                .ValidateDataAnnotations();

            services.AddSingleton<SigningService>();

            // Load hosted service to load the certificate files
            services.AddHostedService<LoadCertificatesHostedService>();

            // Add controllers and set some json options
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<ServerOptions> serverOptions)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
            {
                builder.WithOrigins(serverOptions.Value.AllowedOrigins.Split(','))
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}