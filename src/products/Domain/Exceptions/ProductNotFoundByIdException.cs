using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class ProductNotFoundByIdException : ProductNotFoundException
    {
        public ProductNotFoundByIdException(int productId) : base($"Product with ID {productId} was not found")
        {
        }
    }
}
