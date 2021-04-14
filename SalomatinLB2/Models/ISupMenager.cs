using System;
using System.Collections.Generic;


namespace SalomatinLB2.Models
{
    public interface ISupMenager
    {
        List<Shop> SelectShopswithVolumes(List<Shop> shops, int volumesfrom, int volumesto);
        List<Shop> SelectShopswithStocksAdress(List<Shop> shops, string adress);
        List<Shop> SelectShopsContainStock(List<Shop> shops, long id);
        List<Shop> SelectTopShop(List<Shop> shops);
    }
}
