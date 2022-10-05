using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace DotNetExperiments.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly HttpClient _httpClient;
        public IEnumerable<WeatherForecast> WeatherMessages { get; set; } = Enumerable.Empty<WeatherForecast>();

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("API");
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("https://host.docker.internal:8443/WeatherForecast/GetWeatherForecast").ConfigureAwait(false);
                if (response != null)
                {
                    WeatherMessages = JsonConvert.DeserializeObject<IEnumerable<WeatherForecast>>(response);
                }
                else
                {
                    WeatherMessages = Enumerable.Empty<WeatherForecast>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting weather forecast");
            }
            return Page();
        }
    }
}