using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken);

        IQueryable<Product> GetAll();

        Task<IEnumerable<Product>> GetAllAvailableAsync(CancellationToken cancellationToken);

        Task<Product> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<Product> GetByNameAsync(string name, CancellationToken cancellationToken);

        Task CreateAsync(Product product, CancellationToken cancellation);

        Task UpdateAsync(Product product, CancellationToken cancellationToken);

        Task DeleteUserProducts(int userId, CancellationToken cancellationToken);

        Task DeleteAsyncById(int id, CancellationToken cancellationToken);
    }
}
