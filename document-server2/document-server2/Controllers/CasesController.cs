using document_server2.Controllers.BaseController;
using document_server2.Core.Domain.Context;
using document_server2.Infrastructure.Comends;
using document_server2.Infrastructure.DTO;
using document_server2.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using document_server2.Core.Domain;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace document_server2.Controllers
{
    public class CasesController : ApiControllerBase
    {
        private readonly IUserService _userService;
        private readonly DataBaseContext _context;
        private readonly IMemoryCache _cache;

        public CasesController(IUserService userService, DataBaseContext context, IMemoryCache cache)
        {
            _userService = userService;
            _context = context;
            _cache = cache;
        }

        // GET: api/cases
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CaseDTO>>> GetCases()
            => Json(await _userService.GetAllUserCaseAsync(UserEmail));

        // GET: api/cases/spam
        [HttpGet("/spam")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CaseDTO>>> Spam()
        {
            IEnumerable<Case> cases = await _context.Cases.Where(x => x.Status == "spam").ToListAsync();
            return Json(cases);
        }


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

        // GET: api/cases/complaint:desc
        [HttpGet("{type}/{sort}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CaseDTO>>> GetFilterCase(string type, string sort)
            => Json(await _userService.GetFilterCaseAsync(UserEmail, type, sort));



        // GET: api/cases/application
        [HttpGet("/application")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CaseDTO>>> Podanie()
        {
            IEnumerable<Case> cases = await _context.Cases.Where(x => x.Status == "not considered" && x.Type == "podanie").ToListAsync();
            return Json(cases);
        }

        // GET: api/cases/complaint
        [HttpGet("/complaint")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CaseDTO>>> Skarga()
        {
            IEnumerable<Case> cases = await _context.Cases.Where(x => x.Status == "not considered" && x.Type == "skarga").ToListAsync();
            return Json(cases);
        }


        // POST: api/cases
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> PostCase([FromBody] CreateCase @case)
        {
            await _userService.AddCaseAsync(UserEmail, @case);
            return Created("/cases", null);
        }

        // PUT: api/cases/id
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> SetComment(int id, [FromBody] UpdateCase data)
        {
            CaseDetailsDTO @case = await _userService.GetCaseAsync(id);

            if (@case == null)
            {
                return NotFound();
            }

            await _userService.UpdateCaseAsync(id, data);
            return NoContent();
        }
        
     
    }
}
