using AutoMapper;
using document_server2.Core.Domain;
using document_server2.Core.Repositories;
using document_server2.Infrastructure.Comends;
using document_server2.Infrastructure.DTO;
using document_server2.Infrastructure.Services.JwtToken;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<UserDTO> GetByLoginAsync(string email)
            => _mapper.Map<UserDTO>(await _userRepository.GetByEmailAsync(email));

        public async Task<UserDTO> GetByEmailAsync(string login)
            => _mapper.Map<UserDTO>(await _userRepository.GetByLoginAsync(login));

        public async Task RegisterAsync(CreateUser data)
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

            user = new User(data.Email, data.Login, data.Password, data.Role);
            await _userRepository.AddAsync(user);
        }

        public async Task<LoginDTO> LoginAsync(string email, string password)
        {
            User user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("Invalid credentials.");
            }

            if(user.Password != password)
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

        public async Task<IEnumerable<CaseDTO>> GetAllUserCaseAsync(string email)
            => _mapper.Map<IEnumerable<CaseDTO>>(await _userRepository.GetAllUserCaseAsync(email));

        public async Task AddCaseAsync(string email, CreateCase @case)
        {
            User user = await _userRepository.GetByEmailAsync(email);

            if (user != null)
            {
                Case new_case = new Case(email, @case.Type, @case.Description, _mapper.Map<IEnumerable<Document>>(@case.Documents));
                user.AddCase(new_case);
                await _userRepository.UpdateAsync(user);
            }
        }
    }
}
