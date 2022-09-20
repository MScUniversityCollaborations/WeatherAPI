using System.Net.Http;
using System.Threading.Tasks;

namespace MakingHttpRequest
{
    public interface IWeatherService
    {
        Task<string> Get(string cityName);
    }

    public class WeatherService : IWeatherService
    {
        private HttpClient _httpClient;

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> Get(string cityName)
        {
            var apiKey = "2f88f13a8557410693e193741221909";

            string APIURL = $"?key={apiKey}&q={cityName}";
            var response = await _httpClient.GetAsync(APIURL);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}