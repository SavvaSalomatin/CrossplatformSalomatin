using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalomatinLB2.Models;

namespace SalomatinLB2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopsController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly ISupMenager _mng;

        public ShopsController(MyContext context, ISupMenager mng)
        {
            _mng = mng;
            _context = context;
        }

        // GET: api/Shops
        //[Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shop>>> GetShops()
        {
            var shops = _context.Shops.Include(p => p.Stocks).ThenInclude(i => i.stock);
            return await _context.Shops.ToArrayAsync();
        }


        // GET: api/TopShop
        //[Authorize]
        [HttpGet("TopShop")]
        public async Task<ActionResult<IEnumerable<Shop>>> GetTopShop()
        {
            var Shops = (_mng.SelectTopShop(_context.Shops.Include(p => p.Stocks).ThenInclude(i => i.stock).ToList())).TakeLast(5);
            if (Shops == null)
                return CreatedAtAction("GetTopShop", new
            {
                result = "Не найдено магазинов, адреса складов которых  "
            });
            else
            {
                return CreatedAtAction("GetTopShop", new
                {
                    result = Shops
                });
            }
        }

        // GET: api/Shops/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Shop>> GetShop(long id)
        {
            var shop = _context.Shops.Include(i => i.Stocks).ThenInclude(u => u.stock).FirstOrDefault(i => i.Id == id);

            if (shop == null)
            {
                return NotFound();
            }

            return shop;
        }

        // GET: api/Shops/withAdress/Ленинский
        [HttpGet("withAdress/{Adress}")]
        //[Authorize(Roles = "admin")]
        public CreatedAtActionResult GetwithAdress(string adress)
        { 
            var shops = _mng.SelectShopswithStocksAdress(_context.Shops.Include(i => i.Stocks).ThenInclude(u => u.stock).ToList(), adress);
            if (shops.Count == 0)
                return CreatedAtAction("GetwithAdress", new
                {
                    result = "Не найдено магазинов, адреса складов которых  " + adress
                }) ;
            else
            {
                return CreatedAtAction("GetwithAdress", new
                {
                    result = shops
                });
            }
        }
        // GET: api/Shops/withVolumes/15/25
        [HttpGet("withVolumes/{v1}/{v2}")]
        //[Authorize(Roles = "admin")]
        public CreatedAtActionResult GetwithVolumes(int v1, int v2)
        {
            var shops = _mng.SelectShopswithVolumes(_context.Shops.Include(i => i.Stocks).ThenInclude(u => u.stock).ToList(), v1, v2);
            if (shops.Count == 0)
                return CreatedAtAction("GetwithVolumes", new
                {
                    result = "Не найдено магазинов, товарная ёмкость которых в пределах {" + v1 + " , " + v2 +"}"
                });
            else
            {
                return CreatedAtAction("GetwithVolumes", new
                {
                    result = shops
                });
            }
        }

        // PUT: api/Shops/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.

        [HttpPut("Changestocktype")]
        //[Authorize(Roles = "user , admin")]
        public async Task<ActionResult<Shop>> ChangeStockType([FromForm] long id, [FromForm] long id2, [FromForm] string typeprod)
        {
            var shop = _context.Shops.Include(i => i.Stocks).ThenInclude(u => u.stock).FirstOrDefault(i => i.Id == id);
            if (shop != null)
            {
                if (shop.ChangeStockType(id2, typeprod))
                {
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("ChangeStock", new
                    {
                        result = "Данные магазина обновлены."
                    });
                }
                else
                {
                    return CreatedAtAction("ChangeStock", new
                    {
                        result = "В магазине нет такого склада."
                    });
                }
            }
            else
            {
                return CreatedAtAction("ChangeStock", new
                {
                    result = "Проверьте id магазина."
                });

            }
        }


        [HttpPut("Buyproduct")]
        //[Authorize(Roles = "user , admin")]
        public async Task<ActionResult<Shop>> BuyProduct([FromForm] long id, [FromForm] long id2, [FromForm] int vol)
        {
            var shop = _context.Shops.Include(i => i.Stocks).ThenInclude(u => u.stock).FirstOrDefault(i => i.Id == id);
            if (shop != null)
            {
                if (shop.Buy(id2, vol))
                {
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("Buyproduct", new
                    {
                        result = "Данные магазина обновлены."
                    });
                }
                else
                {
                    return CreatedAtAction("Buyproduct", new
                    {
                        result = "В магазине нет такого склада."
                    });
                }
            }
            else
            {
                return CreatedAtAction("Buyproduct", new
                {
                    result = "Проверьте id магазина."
                });

            }
        }


        [HttpPut("Saleproduct")]
        //[Authorize(Roles = "user , admin")]
        public async Task<ActionResult<Shop>> SaleProduct([FromForm] long id, [FromForm] long id2, [FromForm] int vol)
        {
            var shop = _context.Shops.Include(i => i.Stocks).ThenInclude(u => u.stock).FirstOrDefault(i => i.Id == id);
            if (shop != null)
            {
                if (shop.Sale(id2, vol))
                {
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("ChangeStock", new
                    {
                        result = "Данные магазина обновлены."
                    });
                }
                else
                {
                    return CreatedAtAction("ChangeStock", new
                    {
                        result = "В магазине нет такого склада."
                    });
                }
            }
            else
            {
                return CreatedAtAction("ChangeStock", new
                {
                    result = "Проверьте id магазина."
                });

            }
        }


        // POST: api/Shops/Add
        [HttpPost("Add")]
        //[Authorize(Roles = "user , admin")]
        public async Task<ActionResult<Shop>> AddStock([FromForm] long id, [FromForm] long id2, [FromForm] string typeprod)
        {
            var shop = _context.Shops.Include(i => i.Stocks).ThenInclude(u => u.stock).FirstOrDefault(i => i.Id == id);
            if (shop != null)
            {
                var p = shop.Stocks.FirstOrDefault(i => i.stock.Id == id2);
                if (p == null)
                {
                    var StockToAdd = _context.Stocks.FirstOrDefault(r => r.Id == id2);
                    if (StockToAdd != null)
                    {
                        shop.AddStock(StockToAdd, typeprod);
                        await _context.SaveChangesAsync();
                        return CreatedAtAction("AddStock", new
                        {
                            result = "Склад добавлен."
                        });
                    }
                    else
                    {
                        return CreatedAtAction("AddStock", new
                        {
                            result = "Такого склада нет в базе."
                        });
                    }
                }
                else
                {
                    return CreatedAtAction("AddStock", new
                    {
                        result = "Такой склад уже есть."
                    });
                }
            }
            else
            {
                return CreatedAtAction("AddStock", new
                {
                    result = "Магазина с таким номером не найдено."
                });
            }

        }

        // POST: api/Shops
        [HttpPost]
        //[Authorize(Roles = "user , admin")]
        public async Task<ActionResult<Shop>> PostShop(List<ShopsStocks> shopsstocks, string name, string adress)
        {
            Shop shop = new Shop();
            foreach (var i in shopsstocks)
            {
                foreach (Stock stock in _context.Stocks)
                {
                    if (i.Id == stock.Id)
                    {
                        ShopsStocks Stock = new ShopsStocks();
                        Stock.stock = stock;
                        Stock.Typeofproduct = i.Typeofproduct;
                        shop.Stocks.Add(Stock);
                    }
                }
            }
            if (shop.Stocks.Count != 0)
            {
                shop.Name = name;
                shop.Adress = adress;
                _context.Shops.Add(shop);
                await _context.SaveChangesAsync();
                return CreatedAtAction("PostShop", new { id = shop.Id }, shop);
            }
            else
            {
                return CreatedAtAction("PostShopr", new
                {
                    result = "Добавляемых складов нет в базе."
                });
            }
        }


        // DELETE: api/Shops/5
        [HttpDelete("{id}")]
        //[Authorize(Roles = "user , admin")]
        public async Task<ActionResult<Shop>> DeleteShop(long id)
        {
            var shop = _context.Shops.Include(i => i.Stocks).ThenInclude(u => u.stock).FirstOrDefault(i => i.Id == id);
            if (shop == null)
            {
                return NotFound();
            }

            _context.Shops.Remove(shop);
            await _context.SaveChangesAsync();

            return shop;
        }

        //DELETE:api/Shops/5/Stock/2
        [HttpDelete("{id}/Stock/{id2}")]
        //[Authorize(Roles = "user , admin")]
        public async Task<ActionResult<Shop>> DeleteStock(long id, long id2)
        {
            var shop = _context.Shops.Include(i => i.Stocks).ThenInclude(u => u.stock).FirstOrDefault(i => i.Id == id);
            if (shop != null)
            {
                if (shop.DeleteStock(id2))
                {
                    await _context.SaveChangesAsync();
                    if (shop.Stocks.Count == 0)
                    {
                        _context.Shops.Remove(shop);
                        await _context.SaveChangesAsync();
                        return CreatedAtAction("DeleteStock", new
                        {
                            result = "Склад"+id2+ "удален из магазина"+id
                        });
                    }
                    return CreatedAtAction("DeleteStock", new
                    {
                        result = "Данные магазина обновлены."
                    });
                }
                else
                {
                    return CreatedAtAction("DeleteStock", new
                    {
                        result = "В магазине нет такого склада."
                    });
                }
            }
            else
            {
                return CreatedAtAction("DeleteStock", new
                {
                    result = "Такого магазина не найдено."
                });

            }
        }

        private bool ShopExists(long id)
        {
            return _context.Shops.Any(e => e.Id == id);
        }
    }
}


