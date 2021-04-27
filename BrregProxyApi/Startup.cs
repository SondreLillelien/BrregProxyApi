using System;
using System.Net.Http;
using BrregProxyApi.Options;
using BrregProxyApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace BrregProxyApi
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
            services.Configure<AppSettings>(Configuration);
            services.AddControllers();
            services.AddHttpClient();
            services.AddMemoryCache(options => options.SizeLimit = 128);
            services.AddTransient<IOrgDataService>(provider =>
            {
                var client = provider.GetRequiredService<IHttpClientFactory>().CreateClient();
                var baseUrl = provider.GetRequiredService<IOptionsSnapshot<AppSettings>>()
                    .Value
                    .OrgDataSettings
                    .BaseUrl;
                return new OrgDataService(client, baseUrl);
            });
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo {Title = "BrregProxyApi", Version = "v1"});
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BrregProxyApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}