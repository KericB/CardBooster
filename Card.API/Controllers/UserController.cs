using CardBooster.Core.Commands;
using CardBooster.Core.Commands.Authentication;
using CardBooster.Infrastructure.Command;
using Microsoft.AspNetCore.Mvc;

namespace Card.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ICommandAsyncHandler<RegisterUserCommand> _registerHandler;
        private readonly ICommandAsyncHandler<LoginUserCommand> _loginHandler;
        private readonly ICommandHandler<AddUserCommand> _addUserHandler;

        public UserController(ICommandHandler<AddUserCommand> addUserHandler)
        {
            _addUserHandler = addUserHandler;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] AddUserCommand command)
        {
            var result = _addUserHandler.Execute(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
