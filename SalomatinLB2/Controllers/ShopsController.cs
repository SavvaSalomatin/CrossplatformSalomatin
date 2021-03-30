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

        public ShopsController(MyContext context)
        {
            _context = context;
        }

        // GET: api/Shops
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shop>>> GetShops()
        {
            return await _context.Shops.ToListAsync();
        }

        // GET: api/Shops/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Shop>> GetShop(long id)
        {
            var shop = await _context.Shops.FindAsync(id);

            if (shop == null)
            {
                return NotFound();
            }

            return shop;
        }

        // PUT: api/Shops/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShop(long id, Shop shop)
        {
            if (id != shop.Id)
            {
                return BadRequest();
            }

            _context.Entry(shop).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShopExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Shops
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Shop>> PostShop(Shop shop)
        {
            _context.Shops.Add(shop);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShop", new { id = shop.Id }, shop);
        }

        // DELETE: api/Shops/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Shop>> DeleteShop(long id)
        {
            var shop = await _context.Shops.FindAsync(id);
            if (shop == null)
            {
                return NotFound();
            }

            _context.Shops.Remove(shop);
            await _context.SaveChangesAsync();

            return shop;
        }

        [HttpGet("StockId/{Id}")]
        public async Task<ActionResult<IEnumerable<Shop>>> GetStocks(int v)
        {
            return await _context.Shops.Where(u => u.StockId == v).ToListAsync();
        }

        [HttpGet("StockAdress/{Id}")]
        IEnumerable<ShopfromStock> GetShopList(IEnumerable<Shop> tblshops, IEnumerable<Stock> tblstocks)
        {
            var result = new List<ShopfromStock>();
            foreach (Stock stock in tblstocks)
            {
                var match = tblshops.FirstOrDefault((r) => r.Adress.Contains(stock.Adress));
                if (match != null)
                {
                    result.Add(new ShopfromStock() { Id = stock.Id, Adress = match.Adress });
                }
            }
            return result;
        }

        private bool ShopExists(long id)
        {
            return _context.Shops.Any(e => e.Id == id);
        }



        class ShopfromStock
        {
            public long Id { get; set; }
            public string Adress { get; set; }
        }
    }
}


