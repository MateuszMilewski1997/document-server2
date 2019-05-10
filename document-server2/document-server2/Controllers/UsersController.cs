using document_server2.Controllers.BaseController;
using document_server2.Infrastructure.Comends;
using document_server2.Infrastructure.DTO;
using document_server2.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace document_server2.Controllers
{
    public class UsersController : ApiControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: api/users/registration
        [HttpPost("registration")]
        public async Task<ActionResult> Register([FromBody] CreateUser data)
        {
            await _userService.RegisterAsync(data);
            return Created("/users", null);
        }

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<ActionResult<LoginDTO>> Login([FromBody] Login command)
            => Json(await _userService.LoginAsync(command.Identifier, command.Password));

        // PUT: api/users/email
        [HttpPut("{email}")]
        [Authorize]
        public async Task<ActionResult> PutEvent(string email, [FromBody] UpdateUser commend)
        {
            if (UserEmail != email)
                return Forbid();
            await _userService.UpdateAsync(email, commend);
            return NoContent();
        }
    }
}
