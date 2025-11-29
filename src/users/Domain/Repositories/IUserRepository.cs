using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        Task<ICollection<User>> GetAllAsync();

        IQueryable<User> GetAll();

        Task<User> GetByIdAsync(int id);

        Task<User> GetByEmailAsync(string email);

        Task CreateAsync(User user);

        Task UpdateAsync(User user);

        Task DeleteByIdAsync(int id);
    }
}
