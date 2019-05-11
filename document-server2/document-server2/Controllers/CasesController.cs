using document_server2.Controllers.BaseController;
using document_server2.Core.Domain;
using document_server2.Core.Domain.Context;
using document_server2.Infrastructure.Comends;
using document_server2.Infrastructure.DTO;
using document_server2.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Mvc.Filters;

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

        // POST: api/cases
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> PostCase([FromBody] CreateCase @case)
        {
            await _userService.AddCaseAsync(UserEmail, @case);
            await _userService.SendEmailAsync(UserEmail, @case);
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

        [HttpPost("sendtoken/{email}")]
        public async Task<ActionResult> Sendtoken(string email)
        {
           // if (_context.Cases.Any(e => e.User_Mail == email))
           // {

                var random = new Random();

                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var stringChars = new char[8];


                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }

                string key = new String(stringChars);

                
                _cache.Set("key", key, TimeSpan.FromMinutes(180));
                SmtpClient client = new SmtpClient();
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                client.Host = "smtp.gmail.com";
                client.Port = 587;

                // setup Smtp authentication
                System.Net.NetworkCredential credentials =
                    new System.Net.NetworkCredential("FakultetBillenium@gmail.com", "haslo4321");
                client.UseDefaultCredentials = false;
                client.Credentials = credentials;

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress("FakultetBillenium@gmail.com");

                //msg.To.Add(new MailAddress("milewskimateusz28@gmail.com"));
                msg.To.Add(new MailAddress(email));


                msg.Subject = "System zarządzania obiegiem dokumentów";
                msg.IsBodyHtml = true;
                msg.Body = string.Format($"<html><head></head><body><b>Twój klucz: {key} </b>   </body>");

                client.Send(msg);
           // }

            return Ok();
        }

        
      /*  // POST: api/cases
        [HttpPost("{id}/{url}/{filename}")]
        [Authorize]
        public async Task<ActionResult<Document>> EditCase(int id, string url, string filename)
        {
            var @case = await _context.Cases.FindAsync(id);
              

            if( @case != null )
            {

                Document doc = new Document()
                {
                   Case_id = id,
                   Name = filename,
                   Url = url

                };

                _context.Users.Add(doc);
                await _context.SaveChangesAsync();
            }
            else
            {
                return NoContent();
            }

            return Created("/cases", null);
        }*/

    }
}
