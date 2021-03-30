using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace SalomatinLB2.Models
{
    public class MyContext : DbContext
    {
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        //public DbSet<User> Users { get; set; }

        public MyContext(DbContextOptions<MyContext> options)
        : base(options)
        {

            Database.EnsureCreated();
        }
    }
}
