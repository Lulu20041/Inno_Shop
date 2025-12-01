using Application.Interfaces;
using Domain;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository repo;

        public ProductService(IProductRepository repo) { this.repo = repo; }

        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await repo.GetAllAsync(cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetAllAvailableAsync(CancellationToken cancellationToken)
        {
            return await repo.GetAllAvailableAsync(cancellationToken);
        }

        public async Task<Product> GetByIdAsync(int id,CancellationToken cancellationToken)
        {
            return await repo.GetByIdAsync(id, cancellationToken);
        }

        public async Task<Product> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await repo.GetByNameAsync(name, cancellationToken);
        }

        public async Task AddAsync(Product product, CancellationToken cancellationToken)
        {
            await repo.CreateAsync(product, cancellationToken);
        }

        public async Task DeleteByIdAsync(int id, CancellationToken cancellationToken)
        {
            await repo.DeleteAsyncById(id, cancellationToken);
        }

        public async Task UpdateAsync(Product product, CancellationToken cancellationToken)
        {
            await repo.UpdateAsync(product, cancellationToken);
        }
    }
}
