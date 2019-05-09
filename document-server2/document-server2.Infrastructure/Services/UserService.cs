using AutoMapper;
using document_server2.Core.Repositories;
using document_server2.Infrastructure.Comends;
using document_server2.Infrastructure.DTO;
using System;
using System.Threading.Tasks;

namespace document_server2.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDTO> GetByLoginAsync(string email)
            => _mapper.Map<UserDTO>(await _userRepository.GetByEmailAsync(email));

        public async Task<UserDTO> GetByEmailAsync(string login)
            => _mapper.Map<UserDTO>(await _userRepository.GetByLoginAsync(login));

        public async Task RegisterAsync(CreateUser data)
        {
            throw new NotImplementedException();
        }

        public async Task<LoginDTO> LoginAsync(string identifier, string password)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(string identifier, CreateUser data)
        {
            throw new NotImplementedException();
        }
    }
}
