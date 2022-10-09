using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Polly;

namespace WeatherAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(new ActionAsyncFilterAttribute(""));
            });

            services.AddHealthChecksUI()
                .AddInMemoryStorage()
                .Services
                .AddHealthChecks()
                .AddApplicationInsightsPublisher();
                //.AddUrlGroup(new Uri("http://api.weatherapi.com/v1/current.json"),"WeatherAPI"); 

            services.AddHealthChecksUI()
                .AddInMemoryStorage();
            
            var timeout = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(5));
            
            services.AddHttpClient<IWeatherService, WeatherService>(c =>
            {
                c.BaseAddress = new Uri("http://api.weatherapi.com/v1/current.json");
            }).AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(2)))
                .AddTransientHttpErrorPolicy(policy => policy.CircuitBreakerAsync(5, TimeSpan.FromSeconds(5)))
                .AddPolicyHandler(request =>
                {
                    if (request.Method == HttpMethod.Get)
                        return timeout;

                    return Policy.NoOpAsync<HttpResponseMessage>();
                });

            // Alternatively you can also add extension methods like below to keep this method clean
            // services.AddWeatherService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            /*app.UseHealthChecks("/health");*/
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseHealthChecks("/healthcheck", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse //nuget: AspNetCore.HealthChecks.UI.Client
            });
    
            //nuget: AspNetCore.HealthChecks.UI
            app.UseHealthChecksUI(options =>
            {
                options.UIPath = "/healthchecks-ui";
                options.ApiPath = "/health-ui-api";
            });
            
            app.UseEndpoints(endpoints =>
            {
                /*endpoints.MapHealthChecks("api/health");
                endpoints.MapHealthChecksUI(config => {
                        config.UIPath = "/hc-ui";
                    });*/
                
                endpoints.MapControllers();
            });
        }
        
        /*public class RandomHealthCheck : IHealthCheck
        {
            public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
            {
                if (DateTime.UtcNow.Minute % 2 == 0)
                {
                    return Task.FromResult(HealthCheckResult.Healthy());
                }

                return Task.FromResult(HealthCheckResult.Unhealthy(description: "failed"));
            }
        }*/
    }
}
