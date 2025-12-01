using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException() : base($"Product was not found")
        {
        }

        public ProductNotFoundException(string message) : base(message)
        {
        }
    }
}
