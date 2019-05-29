using document_server2.Infrastructure.Comends;
using document_server2.Infrastructure.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace document_server2.Infrastructure.Services
{
    public interface IUserService
    {
        Task<UserDTO> GetByLoginAsync(string email);
        Task<UserDTO> GetByEmailAsync(string login);
        Task RegisterAsync(CreateUser data, string type);
        Task<LoginDTO> LoginAsync(string identity, string password);
        Task UpdateAsync(string email, UpdateUser data);
        Task UpdateUserByAdminAsync(string email, UpdateUser data);
        Task<CaseDetailsDTO> GetCaseAsync(int id);
        Task<IEnumerable<CaseDTO>> GetFilterCaseAsync(string email, string type, string sort);
        Task<IEnumerable<CaseDTO>> GetAllUserCaseAsync(string email);
        Task AddCaseAsync(string email, CreateCase @case);
        Task UpdateCaseAsync(int id, UpdateCase data);
        Task SendEmailAsync(string sender_email, CreateCase @case);
    }
}
