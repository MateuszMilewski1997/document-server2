using document_server2.Core.Domain;
using System.Threading.Tasks;

namespace document_server2.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByLoginAsync(string login);
        Task AddAsync(User User);
        Task UpdateAsync(User User);
    }
}
