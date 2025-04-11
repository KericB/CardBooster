using CardBooster.Blazor.Models;
using CardBooster.Core.Models;

namespace CardBooster.Blazor.Services.Interface
{
    public interface IBoosterService
    {
        Task<BoosterModel?> OpenBoosterAsync();
        Task<List<Cards>> GetUserCardsAsync();


    }
}
