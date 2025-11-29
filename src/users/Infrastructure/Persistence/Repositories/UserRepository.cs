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

        public async Task<ICollection<User>> GetAllAsync() => await context.Users.AsNoTracking().ToListAsync();

        public async Task<User> GetByIdAsync(int id)
        {
            return await context
                .Users
                .FindAsync(id);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await context
                .Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task CreateAsync(User user)
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            var newUser = user;
            newUser.Name = user.Name;
            newUser.Email = user.Email;
            newUser.HashPassword = user.HashPassword;
            await context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var user = await context.Users.FindAsync(id);
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }
    }
}
