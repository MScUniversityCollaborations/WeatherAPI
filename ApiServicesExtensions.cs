using System;
using Microsoft.Extensions.DependencyInjection;

namespace MakingHttpRequest
{
    public static class ApiServicesExtensions
    {
        public static void AddWeatherService(this IServiceCollection services)
        {
            services.AddHttpClient<IWeatherService, WeatherService>(c =>
            {
                c.BaseAddress = new Uri("http://api.weatherapi.com/v1/current.json");
            });
        }
    }
}