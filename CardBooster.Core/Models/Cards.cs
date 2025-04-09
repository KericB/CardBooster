using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardBooster.Core.Models
{
    public class Cards
    {
        public int IdCard { get; set; }
        public string Name { get; set; }
        public string Rarity { get; set; }
        public string IdRarity { get; set; }

        public string ImageUrl { get; set; }
    }
}
