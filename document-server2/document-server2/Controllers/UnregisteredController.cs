﻿using document_server2.Controllers.BaseController;
using document_server2.Infrastructure.Comends;
using document_server2.Infrastructure.DTO;
using document_server2.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace document_server2.Controllers
{
    public class UnregisteredController : ApiControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMemoryCache _cache;

        public UnregisteredController(IUserService userService, IMemoryCache cache)
        {
            _userService = userService;
            _cache = cache;
        }

        // POST: api/unregistered/sendtoken/email
        [HttpPost("sendtoken/{email}")]
        public async Task<ActionResult> Sendtoken(string email)
        {
            UserDTO user = await _userService.GetByEmailAsync(email);

            if (user == null)
            {
                await _userService.RegisterAsync(new CreateUser() { Email = email }, "unregistered");
            }

            var random = new Random();

            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var stringChars = new char[8];

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            string key = new string(stringChars);

            _cache.Set("key", $"{email}-{key}", TimeSpan.FromMinutes(180));
            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;

            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential("FakultetBillenium@gmail.com", "haslo4321");
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;

            MailMessage message = new MailMessage();
            message.From = new MailAddress("FakultetBillenium@gmail.com");

            message.To.Add(new MailAddress(email));

            message.Subject = "System zarządzania obiegiem dokumentów";
            message.IsBodyHtml = true;
            message.Body = string.Format($"<html><head></head><body><b>Twój klucz: {key}</b></body></html>");

            client.Send(message);

            return Created("/unregistered/cases", null);
        }

        [HttpGet("cases/{email}/{token}")]
        public async Task<ActionResult> GetCase(string email, string token)
        {
            UserDTO user = await _userService.GetByEmailAsync(email);

            if (user == null)
            {
                return NotFound();
            }

            string key = _cache.Get<string>("key");

            if ($"{email}-{token}" != key)
            {
                return NotFound();
            }

            return Json(await _userService.GetAllUserCaseAsync(email));
        }
    }
}
