using AutoMapper;
using document_server2.Core.Domain;
using document_server2.Core.Repositories;
using document_server2.Infrastructure.Comends;
using document_server2.Infrastructure.DTO;
using document_server2.Infrastructure.Services.JwtToken;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using static document_server2.Infrastructure.Comends.CreateCase;

namespace document_server2.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtHandler _jwtHandler;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper, IJwtHandler jwtHandler)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtHandler = jwtHandler;
        }

        public async Task<UserDTO> GetByEmailAsync(string email)
            => _mapper.Map<UserDTO>(await _userRepository.GetByEmailAsync(email));

        public async Task<UserDTO> GetByLoginAsync(string login)
            => _mapper.Map<UserDTO>(await _userRepository.GetByLoginAsync(login));

        public async Task RegisterAsync(CreateUser data, string type)
        {
            User user = await _userRepository.GetByEmailAsync(data.Email);
            if (user != null)
            {
                throw new Exception($"User e-mail: '{data.Email}' already exists.");
            }

            user = await _userRepository.GetByLoginAsync($"User login: '{data.Login}' already exists.");
            if (user != null)
            {
                throw new Exception($"User e-mail: '{data.Email}' already exists.");
            }

            if(type == "registered")
            {
                user = new User(data.Email, data.Login, data.Password, data.Role);
            }
            else if(type == "unregistered")
            {
                user = new User(data.Email, "unregistered");
            }
            await _userRepository.AddAsync(user);
        }

        public async Task<LoginDTO> LoginAsync(string identity, string password)
        {
            User user = await _userRepository.GetByEmailAsync(identity);
            if (user == null)
            {
                user = await _userRepository.GetByLoginAsync(identity);

                if (user == null)
                {
                    throw new Exception("Invalid credentials.");
                }
            }

            if (user.Password != password)
            {
                throw new Exception("Invalid credentials.");
            }

            string token = _jwtHandler.CreateToken(user.Email, user.Role_name);

            return new LoginDTO
            {
                Token = token,
                Login = user.Login,
                Email = user.Email,
                Role = user.Role_name
            };
        }

        public async Task UpdateAsync(string email, UpdateUser data)
        {
            User user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("Invalid credentials.");
            }

            if (user.Password != data.Password)
            {
                throw new Exception("Invalid credentials.");
            }

            if(data.Login != null)
                user.SetLogin(data.Login);
            if (data.NewPassword != null)
                user.SetPassword(data.NewPassword);
            if (data.Role != null)
                user.SetRole(data.Role);

            await _userRepository.UpdateAsync(user);
        }

        public async Task<CaseDetailsDTO> GetCaseAsync(int id)
            => _mapper.Map<CaseDetailsDTO>(await _userRepository.GetCaseAsync(id));

        public async Task<IEnumerable<CaseDTO>> GetFilterCaseAsync(string email, string type, string sort)
            => _mapper.Map<IEnumerable<CaseDTO>>(await _userRepository.GetFilterUserCasesAsync(email, type, sort));

        public async Task<IEnumerable<CaseDTO>> GetAllUserCaseAsync(string email)
            => _mapper.Map<IEnumerable<CaseDTO>>(await _userRepository.GetAllUserCaseAsync(email));

        public async Task AddCaseAsync(string email, CreateCase @case)
        {
            User user = await _userRepository.GetByEmailAsync(email);
            if (user != null)
            {
                Case new_case = new Case(@case.Title, email, @case.Type, @case.Description, _mapper.Map<IEnumerable<Document>>(@case.Documents));
                user.AddCase(new_case);
                await _userRepository.UpdateAsync(user);
            }
        }

        public async Task UpdateCaseAsync(int id, UpdateCase data)
        {
            Case @case = await _userRepository.GetCaseAsync(id);
            if(@case != null)
            {
                if(data.Comment != null)
                    @case.SetComment(data.Comment);
                if (data.Status != null)
                    @case.SetStatus(data.Status);

                if(data.Documents != null)
                {
                    foreach (var document in data.Documents)
                    {
                        @case.AddDocument(document.Name, document.Url);
                    }
                }
            }
            await _userRepository.UpdateCaseAsync(@case);
        }

        public async Task SendEmailAsync(string sender_email, CreateCase @case)
        {
            SmtpClient client = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587
            };

            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential("FakultetBillenium@gmail.com", "haslo4321");
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;

            MailMessage message = new MailMessage
            {
                From = new MailAddress("FakultetBillenium@gmail.com")
            };

            StringBuilder documents  = new StringBuilder();

            if(@case.Documents == null)
            {
                documents.Append("Ta sprawa nie posiada żadnych dodanych dokumentów.");
            }
            else
            {
                foreach (SendDocument document in @case.Documents)
                {
                    documents.Append($"<li><span class=\"text\"><a href=\"{document.Url}\">{document.Name}</a></span></li>");
                }
            }
            
            message.Subject = "System obsługi dokumentów";
            message.IsBodyHtml = true;
            message.Body = string.Format($"<html><head></head><body><table><tr><td valign = \"top\" width= \"33.333%\" style= \"padding-top: 20px;\"><table role= \"presentation\" cellspacing= \"0\" cellpadding= \"0\" border= \"0\" width= \"100%\"><tr><td style= \"text-align: left; padding-left: 5px; padding-right: 5px;\"><h3 class=\"heading\">Użytkownik o adresie e-mail {sender_email} dodał nową sprawę o treści:</h3>{@case.Description}<h4 class=\"heading\">Lista dokumentów:</h4><ul>{documents.ToString()}</ul></td></tr></table></body></head>");
            client.Send(message);

            await Task.CompletedTask;
        }
    }
}
