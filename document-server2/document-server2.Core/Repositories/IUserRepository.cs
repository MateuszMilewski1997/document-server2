using document_server2.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace document_server2.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByLoginAsync(string login);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task<Case> GetCaseAsync(int id);
        //Task<IEnumerable<Case>> GetFilterUserCaseAsync(string email, string type, string sort);
        Task<IEnumerable<Case>> GetAllUserCaseAsync(string email);
        Task UpdateCaseAsync(Case @case);
    }
}
