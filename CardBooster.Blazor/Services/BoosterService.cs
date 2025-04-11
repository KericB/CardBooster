using CardBooster.Blazor.Models;
using CardBooster.Blazor.Services.Interface;
using CardBooster.Core.Models;
using System.Net.Http.Json;

namespace CardBooster.Blazor.Services
{
    public class BoosterService : IBoosterService
    {
        private readonly HttpClient _httpClient;
        
        public BoosterService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<BoosterModel?> OpenBoosterAsync()
        {
            try
            {
                var response = await _httpClient.PostAsync("api/booster/open", null);

                if(response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<BoosterModel>();
                }
                return null;
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error opening booster: {ex.Message}");
                return null;
            }
        }


        public async Task<List<Cards>> GetUserCardsAsync()
        {
            try
            {
                var cards = await _httpClient.GetFromJsonAsync<List<Cards>>("api/booster/cards");
                return cards ?? new List<Cards>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user cards: {ex.Message}");
                return new List<Cards>();
            }
        }

    }
}
