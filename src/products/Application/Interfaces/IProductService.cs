using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductService
    {
        public Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken);

        public Task<IEnumerable<Product>> GetAllAvailableAsync(CancellationToken cancellationToken);

        public Task<Product> GetByIdAsync(int id, CancellationToken cancellationToken);

        public Task<Product> GetByNameAsync(string name, CancellationToken cancellationToken);

        public Task AddAsync(Product product, CancellationToken cancellationToken);

        public Task UpdateAsync(Product product, CancellationToken  cancellationToken);

        public Task DeleteByIdAsync(int id, CancellationToken cancellationToken);
    }
}
