using Application.Interfaces;
using Domain;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository repo;
        private readonly IPasswordHasher hasher;
        private readonly IJwtProvider jwtProvider;

        public UserService(IUserRepository repo, IPasswordHasher hasher, IJwtProvider jwtProvider)
        {
            this.repo = repo;
            this.hasher = hasher;
            this.jwtProvider = jwtProvider;
        }

        public async Task<ICollection<User>> GetAllAsync() => await repo.GetAllAsync();

        public async Task<User> GetByIdAsync(int id) => await repo.GetByIdAsync(id);

        public async Task Register(string name, string email, string pasword)
        {
            string hashPassword = hasher.Generate(pasword);
            User user = new User
            {
                Name = name,
                Email = email,
                HashPassword = hashPassword
            };
            await repo.CreateAsync(user);
        }

        public async Task<string> Login(string email, string password)
        {
            User user = await repo.GetByEmailAsync(email);

            bool result = hasher.Verify(password, user.HashPassword);

            if (result == false)
            {
                throw new Exception("Failed to login");
            }

            string token = jwtProvider.GenerateToken(user);
            return token;
        }

        public async Task UpdateAsync(User user) => await repo.UpdateAsync(user);

        public async Task DeleteByIdAsync(int id) => await repo.DeleteByIdAsync(id);
    }
}
