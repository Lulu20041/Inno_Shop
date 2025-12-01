using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UsersContext context;

        public UserRepository(UsersContext context) => this.context = context;

        public IQueryable<User> GetAll() => context.Users.AsNoTracking();

        public async Task<ICollection<User>> GetAllAsync(CancellationToken cancellationToken) => await context.Users.AsNoTracking().ToListAsync(cancellationToken);

        public async Task<User> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await context
                .Users
                .FindAsync(id, cancellationToken);
        }

        public async Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await context
                .Users
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<User> GetByPasswordResetTokenAsync(string token, CancellationToken cancellationToken)
        {
            return await context
                .Users
                .FirstOrDefaultAsync(u => u.EmailConfirmationToken == token, cancellationToken);
        }

        public async Task CreateAsync(User user , CancellationToken cancellationToken)
        {
            await context.Users.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken)
        {
            var newUser = await context.Users.FirstOrDefaultAsync(u => u.Id == user.Id, cancellationToken);
            newUser.Name = user.Name;
            newUser.Email = user.Email;
            newUser.HashPassword = user.HashPassword;
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteByIdAsync(int id, CancellationToken cancellationToken)
        {
            var user = await context.Users.FindAsync(id, cancellationToken);
            context.Users.Remove(user);
            await context.SaveChangesAsync(cancellationToken);
        }

        
    }
}
