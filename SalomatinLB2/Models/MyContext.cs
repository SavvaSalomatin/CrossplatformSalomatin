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
        public DbSet<ShopsStocks> ShopsStocks { get; set; }
        public DbSet<Stock> Stocks { get; set; }

        public MyContext(DbContextOptions<MyContext> options)
        : base(options)
        {
            //Database.Migrate();
            //Database.EnsureDeleted();
            Database.EnsureCreated();

        }
    }
}
