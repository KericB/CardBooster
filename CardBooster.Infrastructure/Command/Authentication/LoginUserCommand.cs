using CardBooster.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardBooster.Infrastructure.Command.Authentication
{
    internal class LoginUserCommand : ICommandDefinition
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    
}
