using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardBooster.Core.Models
{
    public class Booster
    {
        public int IdBooster { get; set; }
        public DateTime OpenDateBooster { get; set; }
        public int IdUser { get; set; }
        public List<Cards> Cards { get; set; } = new List<Cards>();
    }
}
