using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalomatinLB2.Models
{
    public class SupMenager : ISupMenager
    {
        public List<Shop> SelectShopswithVolumes(List<Shop> shops, int volumefrom, int volumeto)
        {
            var selected_shops = shops.Where(shop => shop.SumOfVolumeShop >= volumefrom && shop.SumOfVolumeShop <= volumeto)
                .ToList();
            return selected_shops;
        }

        public List<Shop> SelectShopswithStocksAdress(List<Shop> shops, string adress)
        {
            var selected_shops = shops.Where(shop => shop.Stocks.Any(i => i.stock.Adress == adress))
               .ToList();
            return selected_shops;
        }

        public List<Shop> SelectShopsContainStock(List<Shop> shops, long id)
        {
            var selected_shops = shops.Where(shop => shop.Stocks.Any(i => i.stock.Id == id))
               .ToList();
            return selected_shops;
        }

        public List<Shop> SelectTopShop(List<Shop> shops)
        {
            var selected_shops = (from u in shops orderby u.SumOfVolumes select u).ToList();
            return selected_shops;
        }
    }
}
