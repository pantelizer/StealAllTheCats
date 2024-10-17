using Microsoft.Extensions.Options;
using StealAllTheCats.Models;

namespace StealAllTheCats.Client
{
    public class GetCatsClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GetCatsClient(HttpClient httpclient, IOptions<CatApiSettings> apiSettings)
        {
            _httpClient = httpclient;
            _apiKey = apiSettings.Value.ApiKey;
        }

        public async Task<List<CatClientModel>> GetCatImages()
        {
            var response = await _httpClient.GetFromJsonAsync<List<CatClientModel>>($"https://api.thecatapi.com/v1/images/search?limit=25&api_key={_apiKey}");

            if (response == null || response.Count == 0)
            {
                throw new ArgumentNullException("The response from TheCatApi was null or empty.");
            }
            return response;
        }
    }
}
