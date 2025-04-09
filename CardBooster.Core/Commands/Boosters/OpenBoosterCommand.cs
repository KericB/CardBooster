using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardBooster.Core.Commands.Boosters
{
    public class OpenBoosterCommand : ICommandDefinition
    {
        public int UserId { get; set; }
    }
}
