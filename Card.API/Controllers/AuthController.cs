using CardBooster.Core.Commands;
using CardBooster.Core.Commands.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Card.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ICommandAsyncHandler<RegisterUserCommand> _registerHandler;
        private readonly ICommandAsyncHandler<LoginUserCommand> _loginHandler;

        public AuthController(
            ICommandAsyncHandler<RegisterUserCommand> registerHandler, 
            ICommandAsyncHandler<LoginUserCommand> loginHandler)
        {
            _registerHandler = registerHandler;
            _loginHandler = loginHandler;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var result = await _loginHandler.ExecuteAsync(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { Error = result.ErrorMessage });
            }
            return Ok(new
            {
                Token = command.AuthenticationResult!.Token,
                Message = "Connexion réussie"
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await _registerHandler.ExecuteAsync(command);
            if (!result.IsSuccess)
            {
                return BadRequest(new { Error = result.ErrorMessage });
            }
            return Ok(new
            {
                Message = "Inscription réussie"
            });
        }


    }
}
