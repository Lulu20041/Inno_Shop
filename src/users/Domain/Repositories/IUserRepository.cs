using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        Task<ICollection<User>> GetAllAsync(CancellationToken cancellationToken);

        IQueryable<User> GetAll();

        Task<User> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken);

        Task<User> GetByPasswordResetTokenAsync(string token, CancellationToken cancellationTokenncellationToken);

        Task CreateAsync(User user, CancellationToken cancellationToken);

        Task UpdateAsync(User user, CancellationToken cancellationToken);

        Task DeleteByIdAsync(int id, CancellationToken cancellationToken);
    }
}
