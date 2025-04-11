using CardBooster.Core.Models;

namespace CardBooster.Blazor.Models
{
    public class BoosterModel
    {
        public int IdBooster { get; set; }
        public DateTime OpenDateBooster { get; set; }
        public List<Cards> Cards { get; set; } = new List<Cards>();
    }
}
