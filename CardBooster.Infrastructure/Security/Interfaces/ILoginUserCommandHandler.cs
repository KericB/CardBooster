using CardBooster.Core.Commands.Authentication;
using CardBooster.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardBooster.Infrastructure.Security.Interfaces
{
    public interface ILoginUserCommandHandler
    {
        Task<Result<AuthenticationResult>> ExecuteAsync(LoginUserCommand command);
    }
}
