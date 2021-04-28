using System;
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
            // config
            services.Configure<AppSettings>(Configuration);
            services.AddTransient(provider => provider.GetRequiredService<IOptions<AppSettings>>().Value);
            
            // mvc
            services.AddControllers();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo {Title = "BrregProxyApi", Version = "v1"});
            });
            
            // services
            services.AddHttpClient<IOrgDataService, OrgDataService>((provider, client) =>
            {
                var baseUrl = provider.GetRequiredService<AppSettings>()
                    .OrgDataSettings
                    .BaseUrl;
                client.BaseAddress = new Uri(baseUrl);
            });
            services.AddMemoryCache(options => options.SizeLimit = 128);
            
            // services.AddTransient<IOrgDataService>(provider =>
            // {
            //     var client = provider.GetRequiredService<IHttpClientFactory>().CreateClient();
            //     var baseUrl = provider.GetRequiredService<AppSettings>()
            //         .OrgDataSettings
            //         .BaseUrl;
            //     return new OrgDataService(client, baseUrl);
            // });

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