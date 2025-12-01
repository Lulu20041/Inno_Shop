using Domain;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductsContext context;

        public ProductRepository(ProductsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await context.Products.AsNoTracking().ToListAsync(cancellationToken);
        }

        public IQueryable<Product> GetAll()
        {
            return context.Products.AsNoTracking();
        }

        public async Task<IEnumerable<Product>> GetAllAvailableAsync(CancellationToken cancellationToken = default)
        {
            return await context.Products
                .Where((p) => p.IsAvailable == true)
                .ToListAsync(cancellationToken);
        }

        public async Task<Product> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await context.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<Product> GetByNameAsync(string name , CancellationToken cancellationToken = default)
        {
            return await context.Products
                .FirstOrDefaultAsync((p) => p.Name == name, cancellationToken);
        }

        public async Task CreateAsync(Product product, CancellationToken cancellationToken = default)
        {
            await context.AddAsync(product, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
        {
            var newProduct = await context.Products.FirstAsync(p => p.Id == product.Id, cancellationToken);
            newProduct.Name = product.Name;
            newProduct.Description = product.Description;
            newProduct.Price = product.Price;
            newProduct.IsAvailable = product.IsAvailable;
            await context.SaveChangesAsync(cancellationToken);
        }

        public Task DeleteAsyncById(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserProducts(int userId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
