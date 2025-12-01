using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<ICollection<User>> GetAllAsync();

        Task<User> GetByIdAsync(int id);

        Task Register(string name, string email, string password);

        Task RegisterAdmin(string name, string email, string password);

        Task<string> Login(string email, string password);

        Task UpdateAsync(User user);

        Task UpdateUserRole(int userId, UserRole role);

        Task DeleteByIdAsync(int id);

        Task<bool> InitiatePasswordReset(string email);

        Task<bool> ResetPassword(string token, string newPassword);

        Task<bool> ConfirmEmail(string token);

        Task DeactivateUserAsync(int userId);

        Task ActivateUserAsync(int userId);
    }
}
