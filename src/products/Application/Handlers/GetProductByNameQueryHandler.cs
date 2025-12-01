using Application.Queries;
using Domain;
using Domain.Repositories;
using Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class GetProductByNameQueryHandler : IRequestHandler<GetProductByNameQuery,Product?>
    {
        private readonly IProductRepository repo;

        public GetProductByNameQueryHandler(IProductRepository productRepository)
        {
            repo = productRepository;
        }

        public async Task<Product?> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
        {
            var product = await repo.GetByNameAsync(request.Name, cancellationToken);
            if (product == null)
                throw new ProductNotFoundException();

            return product;
        }
    }
}
