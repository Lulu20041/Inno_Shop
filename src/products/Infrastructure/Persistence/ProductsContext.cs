using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class ProductsContext : DbContext
    {
        public DbSet<Product> Products {  get; set; }

        public ProductsContext(DbContextOptions<ProductsContext> options) : base(options) { }
    }
}

