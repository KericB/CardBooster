using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardBooster.Core.Commands.Authentication
{
    public class LoginUserCommand : ICommandDefinition
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public AuthenticationResult? AuthenticationResult { get; set; }
    }
}
