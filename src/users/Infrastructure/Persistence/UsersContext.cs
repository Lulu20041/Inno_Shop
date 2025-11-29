using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Infrastructure.Persistence
{
    public class UsersContext : DbContext
    {
        public DbSet<User> Users { get; set; } 

        public UsersContext(DbContextOptions<UsersContext> options) : base(options) { }
    }
}
