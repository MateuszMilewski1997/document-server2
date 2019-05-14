using AutoMapper;
using document_server2.Controllers.BaseController;
using document_server2.Core.Domain;
using document_server2.Core.Domain.Context;
using document_server2.Infrastructure.Comends;
using document_server2.Infrastructure.DTO;
using document_server2.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace document_server2.Controllers
{
    public class UsersController : ApiControllerBase
    {
        private readonly IUserService _userService;
        private readonly DataBaseContext _context;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, DataBaseContext context, IMapper mapper)
        {
            _userService = userService;
            _context = context;
            _mapper = mapper;
        }

        // POST: api/users/registration
        [HttpPost("registration")]
        public async Task<ActionResult> Register([FromBody] CreateUser data)
        {
            await _userService.RegisterAsync(data, "registered");
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

        // GET: api/DropUsers/
        [HttpGet("DropUsers")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> DropUsers()
            => Json(await _context.Users.Where(x => x.Role_name == "droped").ToListAsync());
       

        // GET: api/Users/list
        [HttpGet("List")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserDTO>>> Users()
            => Json( _mapper.Map<IEnumerable<UserDTO>>(await _context.Users.ToListAsync()));


        // GET: api/DropUsers
        [HttpGet("ActiveUsers")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserDTO>>> ActiveUsers()
            => Json(_mapper.Map<IEnumerable<UserDTO>>(await _context.Users.Where(x => x.Role_name != "droped").ToListAsync()));
    }
}
