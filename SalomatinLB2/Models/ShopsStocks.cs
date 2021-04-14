using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalomatinLB2.Models
{
    public class ShopsStocks
    {
        public long Id { get; set; }
        public Stock stock { get; set; }
        public string Typeofproduct { get; set; }
    }
}
