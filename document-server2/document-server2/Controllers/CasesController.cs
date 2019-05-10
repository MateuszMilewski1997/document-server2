using document_server2.Controllers.BaseController;
using document_server2.Infrastructure.Comends;
using document_server2.Infrastructure.DTO;
using document_server2.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace document_server2.Controllers
{
    public class CasesController : ApiControllerBase
    {
        private readonly IUserService _userService;
        public CasesController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/cases
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CaseDTO>>> GetCases()
            => Json(await _userService.GetAllUserCaseAsync(UserEmail));

        // GET: api/cases/id
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<CaseDetailsDTO>> GetCase(int id)
        {
            CaseDetailsDTO @case = await _userService.GetCaseAsync(id);

            if(@case == null)
                NotFound();

            if(@case.User_email != UserEmail)
                Forbid();

            return Json(@case);
        }

        // POST: api/cases
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> PostCase([FromBody] CreateCase @case)
        {
            await _userService.AddCaseAsync(UserEmail, @case);
            return Created("/cases", null);
        }
    }
}
