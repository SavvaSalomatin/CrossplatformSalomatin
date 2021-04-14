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
        public List<ShopsStocks> Stocks { get; set; } = new List<ShopsStocks>();
        private int _sumOfVolumes = 0;
        public int SumOfVolumes
        {
            get
            {
                foreach (ShopsStocks i in Stocks)
                {
                    _sumOfVolumes += i.stock.Volume;
                }
                return _sumOfVolumes;
            }

        }
        public int SumOfVolumeShop => (_sumOfVolumes > 30) ? _sumOfVolumes + 10 : _sumOfVolumes;
        public void AddStock(Stock stock, string Typeprod)
        {
            ShopsStocks Stock = new ShopsStocks();
            Stock.stock = stock;
            Stock.Typeofproduct = Typeprod;
            Stocks.Add(Stock);
        }
        public bool ChangeStockType(long id, string typeprod)
        {
            var stockToChange = Stocks.FirstOrDefault(r => r.Id == id);
            if (stockToChange != null)
            {
                stockToChange.Typeofproduct = typeprod;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Buy(long id, int vol)
        {
            var stockToChange = Stocks.FirstOrDefault(r => r.Id == id);
            if (stockToChange != null)
            {
                stockToChange.stock.Volume = stockToChange.stock.Volume - vol;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Sale(long id, int vol)
        {
            var stockToChange = Stocks.FirstOrDefault(r => r.Id == id);
            if (stockToChange != null)
            {
                stockToChange.stock.Volume = stockToChange.stock.Volume + vol;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteStock(long id)
        {
            var stockToRemove = Stocks.FirstOrDefault(r => r.Id == id);
            if (stockToRemove != null)
            {
                Stocks.Remove(stockToRemove);
                return true;
            }

            else
            {
                return false;
            }
        }
    }
}
