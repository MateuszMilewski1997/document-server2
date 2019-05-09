using document_server2.Core.Domain;
using document_server2.Infrastructure.Comends;
using document_server2.Infrastructure.DTO;
using System.Threading.Tasks;

namespace document_server2.Infrastructure.Services
{
    public interface IUserService
    {
        Task<UserDTO> GetByLoginAsync(string email);
        Task<UserDTO> GetByEmailAsync(string login);
        Task RegisterAsync(CreateUser data);
        Task<LoginDTO> LoginAsync(string identifier, string password);
        Task UpdateAsync(string identifier, CreateUser data);
    }
}
