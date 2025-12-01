using Application.Queries;
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
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product?>
    {
        private readonly IProductRepository repo;

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            repo = productRepository;
        }
        public async Task<Product?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await repo.GetByIdAsync(request.Id, cancellationToken);
            if (product == null)
                throw new ProductNotFoundByIdException(request.Id);

            return product;
        }
    }
}
