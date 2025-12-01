using Application.Interfaces;
using Domain;
using Domain.Exceptions;
using Domain.Repositories;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository repo;
        private readonly IPasswordHasher hasher;
        private readonly IJwtProvider jwtProvider;
        private readonly IEmailConfirmationTokenService tokenService;
        private readonly IEmailService emailService;
        private readonly IOptions<ApplicationOptions> options;

        public UserService(IUserRepository repo, IPasswordHasher hasher, IJwtProvider jwtProvider, IEmailConfirmationTokenService tokenService,IEmailService emailService, IOptions<ApplicationOptions> options)
        {
            this.repo = repo;
            this.hasher = hasher;
            this.jwtProvider = jwtProvider;
            this.tokenService = tokenService;
            this.emailService = emailService;
            this.options = options;
        }

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken) => await repo.GetAllAsync(cancellationToken);

        public async Task<User> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var user = await repo.GetByIdAsync(id, cancellationToken);
            return user == null ? throw new UserNotFoundException(id) : user;
        }

        public async Task Register(string name, string email, string pasword, CancellationToken cancellationToken)
        {
            string hashPassword = hasher.Generate(pasword);
            int hoursToExpire = 4;

            var token = tokenService.GenerateToken();
            var tokenExpires = DateTime.UtcNow.AddHours(hoursToExpire);
            User user = new User
            {
                Name = name,
                Email = email,
                HashPassword = hashPassword,
                IsActive = true,
                IsEmailConfirmed = false,
                CreatedAt = DateTime.UtcNow,
                EmailConfirmationToken = token,
                EmailTokenExpires = tokenExpires
            };
            await repo.CreateAsync(user, cancellationToken);

            var baseUrl = options.Value.PublicUrl.TrimEnd('/');

            var confirmationLink = $"{baseUrl}/auth/confirm-email?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email)}";

            await emailService.SendConfirmationEmailAsync(user.Email, confirmationLink, cancellationToken);
        }

        public async Task RegisterAdmin(string name, string email, string password, CancellationToken cancellationToken)
        {
            string hashPassword = hasher.Generate(password);
            var user = new User
            {
                Name = name,
                Email = email,
                HashPassword = hashPassword,
                IsActive = true,
                IsEmailConfirmed = false,
                CreatedAt = DateTime.UtcNow,
                Role = UserRole.Admin
            };
            await repo.CreateAsync(user, cancellationToken);
        }

        public async Task<string> Login(string email, string password, CancellationToken cancellationToken)
        {
            User user = await repo.GetByEmailAsync(email, cancellationToken) ?? throw new UserNotFoundException(email);
            bool result = hasher.Verify(password, user.HashPassword);

            if (result == false)
            {
                throw new Exception("Invalid credentials");
            }

            string token = jwtProvider.GenerateToken(user);
            return token;
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken) => await repo.UpdateAsync(user, cancellationToken);

        public async Task DeleteByIdAsync(int id, CancellationToken cancellationToken) => await repo.DeleteByIdAsync(id, cancellationToken);

        public async Task<bool> InitiatePasswordReset(string email, CancellationToken cancellationToken)
        {
            var user = await repo.GetByEmailAsync(email, cancellationToken);

            if (user == null)
            {
                return false;
            }

            user.EmailConfirmationToken = Guid.NewGuid().ToString();
            user.EmailTokenExpires = DateTime.UtcNow.AddMinutes(30);

            return true;
        }

        public async Task<bool> ResetPassword(string token, string newPassword, CancellationToken cancellationToken)
        {
            var user = await repo.GetByPasswordResetTokenAsync(token, cancellationToken);

            if (user == null || user.EmailTokenExpires < DateTime.UtcNow)
            {
                return false; 
            }

            user.HashPassword = hasher.Generate(newPassword);
            user.ResetPasswordToken = string.Empty;
            user.ResetTokenExpires = null;

            await repo.UpdateAsync(user, cancellationToken);
            return true;
        }

        public async Task DeactivateUserAsync(int userId, CancellationToken cancellationToken)
        {
            var user = await repo.GetByIdAsync(userId, cancellationToken) ?? throw new UserNotFoundException(userId);
            user.IsActive = false;
            await repo.UpdateAsync(user, cancellationToken);
        }

        public async Task ActivateUserAsync(int userId, CancellationToken cancellationToken)
        {
            var user = await repo.GetByIdAsync(userId, cancellationToken);
            if (user == null)
                throw new UserNotFoundException(userId);

            user.IsActive = true;
            await repo.UpdateAsync(user, cancellationToken);
        }

        public async Task UpdateUserRole(int userId, UserRole role, CancellationToken cancellationToken)
        {
            var user = await repo.GetByIdAsync(userId, cancellationToken) ?? throw new UserNotFoundException(userId);
            if (!Enum.IsDefined(typeof(UserRole), role))
                throw new ArgumentException("Invalid role value");

            user.Role = role;
            await repo.UpdateAsync(user, cancellationToken);
        }
    }
}
