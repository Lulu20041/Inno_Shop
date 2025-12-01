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
        Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken);

        Task<User> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task Register(string name, string email, string password, CancellationToken cancellationToken);

        Task RegisterAdmin(string name, string email, string password, CancellationToken cancellationToken);

        Task<string> Login(string email, string password, CancellationToken cancellationToken);

        Task UpdateAsync(User user, CancellationToken cancellationToken);

        Task UpdateUserRole(int userId, UserRole role, CancellationToken cancellationToken);

        Task DeleteByIdAsync(int id, CancellationToken cancellationToken);

        Task<bool> InitiatePasswordReset(string email, CancellationToken cancellationToken);

        Task<bool> ResetPassword(string token, string newPassword, CancellationToken cancellationToken);

        Task DeactivateUserAsync(int userId, CancellationToken cancellationToken);

        Task ActivateUserAsync(int userId, CancellationToken cancellationToken);
    }
}
