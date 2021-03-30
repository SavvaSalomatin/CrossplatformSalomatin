using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalomatinLB2.Models
{
    public class Shop
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public long StockId { get; set; }
        public Stock Stock { get; set; }
    }
}
