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
                HashPassword = hashPassword,
                IsActive = true,
                IsEmailConfirmed = false,
                CreatedAt = DateTime.UtcNow,
            };
            await repo.CreateAsync(user);
        }

        public async Task<string> Login(string email, string password)
        {
            User user = await repo.GetByEmailAsync(email);

            bool result = hasher.Verify(password, user.HashPassword);

            if (result == false)
            {
                throw new Exception("Invalid credentials");
            }

            string token = jwtProvider.GenerateToken(user);
            return token;
        }

        public async Task UpdateAsync(User user) => await repo.UpdateAsync(user);

        public async Task DeleteByIdAsync(int id) => await repo.DeleteByIdAsync(id);

        public async Task<bool> InitiatePasswordReset(string email)
        {
            var user = await repo.GetByEmailAsync(email);

            if (user == null)
            {
                return false;
            }

            user.PasswordResetToken = Guid.NewGuid().ToString();
            user.ResetTokenExpires = DateTime.UtcNow.AddMinutes(30);

            return true;
        }

        public async Task<bool> ResetPassword(string token, string newPassword)
        {
            var user = await repo.GetByPasswordResetTokenAsync(token);

            if (user == null || user.ResetTokenExpires < DateTime.UtcNow)
            {
                return false; 
            }

            user.HashPassword = hasher.Generate(newPassword);
            user.PasswordResetToken = string.Empty;
            user.ResetTokenExpires = null;

            await repo.UpdateAsync(user);
            return true;
        }

        public Task<bool> ConfirmEmail(string token)
        {
            throw new NotImplementedException();
        }

        public async Task DeactivateUserAsync(int userId)
        {
            var user = await repo.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.IsActive = false;
            await repo.UpdateAsync(user);
        }

        public async Task ActivateUserAsync(int userId)
        {
            var user = await repo.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.IsActive = true;
            await repo.UpdateAsync(user);
        }
    }
}
