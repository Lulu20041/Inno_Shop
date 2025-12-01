using Application.Commands;
using Domain;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, int>
    {
        private readonly IProductRepository repo;

        public UpdateProductCommandHandler(IProductRepository repo) => this.repo = repo;

        public async Task<int> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await repo.GetByIdAsync(request.Id,cancellationToken);
            if (product == null)
                throw new ProductNotFoundByIdException(request.Id);

            if (product.CreatedUserId != request.UserId)
                throw new UnauthorizedAccessException("You can only edit your own products.");

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;

            await repo.UpdateAsync(product, cancellationToken);

            return product.Id;
        }
    }
}
