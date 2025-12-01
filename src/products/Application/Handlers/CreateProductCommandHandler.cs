using System;
using MediatR;
using Domain;
using Application.Commands;
using Domain.Repositories;

namespace Application.Handlers
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IProductRepository repo;

        public CreateProductCommandHandler(IProductRepository repo) => this.repo = repo;
        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                CreatedUserId = request.UserId,
                CreatedAt = DateTime.UtcNow,
                IsAvailable = true
            };

            await repo.CreateAsync(product,cancellationToken);
            return product.Id;
        }
    }
}
