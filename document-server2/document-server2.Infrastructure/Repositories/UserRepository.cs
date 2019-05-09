using document_server2.Core.Domain;
using document_server2.Core.Domain.Context;
using document_server2.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace document_server2.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataBaseContext _context;

        public UserRepository(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<User> GetByEmailAsync(string email)
            => await _context.Users.SingleOrDefaultAsync(user => user.Email == email);

        public async Task<User> GetByLoginAsync(string login)
            => await _context.Users.SingleOrDefaultAsync(user => user.Login == login);

        public async Task AddAsync(User User)
        {
            _context.Users.Add(User);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(User User)
        {
            _context.Users.Update(User);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }
    }
}
