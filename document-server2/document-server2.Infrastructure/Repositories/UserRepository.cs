using document_server2.Core.Domain;
using document_server2.Core.Domain.Context;
using document_server2.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            => await _context.Users
            .Include(x => x.Cases)
                .ThenInclude(x => x.Documents)
            .Include(x => x.Role)
            .SingleOrDefaultAsync(user => user.Email.Equals(email));

        public async Task<User> GetByLoginAsync(string login)
            => await _context.Users
            .Include(x => x.Cases)
                .ThenInclude(x => x.Documents)
            .Include(x => x.Role)
            .SingleOrDefaultAsync(user => user.Login.Equals(login));

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task<Case> GetCaseAsync(int id)
            => await _context.Cases.Include(x => x.Documents).SingleOrDefaultAsync(@case => @case.Id == id);

        public async Task<IEnumerable<Case>> GetFilterUserCasesAsync(string email, string type, string sort)
        {
            if (sort == "desc")
            {
                return await _context.Cases.Include(x => x.Documents).Where(x => x.Type == type && x.User_email == email)
                    .OrderByDescending(x => x.Type).ToListAsync();
            }
            else if(sort == "asc")
            {
                return await _context.Cases.Include(x => x.Documents).Where(x => x.Type == type && x.User_email == email)
                    .OrderBy(x => x.Type).ToListAsync();
            }

            return null;
        }

        public async Task<IEnumerable<Case>> GetAllUserCaseAsync(string email)
            => await _context.Cases.Where(@case => @case.User_email == email).ToListAsync();

        public async Task UpdateCaseAsync(Case @case)
        {
            _context.Cases.Update(@case);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }
    }
}
