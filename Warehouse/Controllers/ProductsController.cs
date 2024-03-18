using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data;
using Warehouse.Models;

namespace Warehouse.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly WarehouseContext _context;

        public ProductsController(WarehouseContext context)
        {
            _context = context;
        }

       

        // GET: Products
        public async Task<IActionResult> Index(String searchString)
        {
            var products = from m in _context.Products select m;
            //Console.WriteLine("搜尋所有產品集合" + products);
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.ProductName.Contains(searchString));
                //Console.WriteLine("搜尋結果產品集合" + products);
            }
            return View(await products.ToListAsync());

            //--純顯示Indext view 返回Products List
            //return _context.Products != null ? 
            //            View(await _context.Products.ToListAsync()) :
            //            Problem("Entity set 'WarehouseContext.Product'  is null.");
        }

        // GET: Products/Details/
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,CategoryID,UnitPrice,QuantityPerUnit")] Product product)
        {
            if(!ModelState.IsValid)
            {
                Console.WriteLine("ERROR");
                
            }
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        //postman 測試create
        [HttpPost]
        public async Task<IActionResult> Create2([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ERROR");

            }
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,CategoryID,UnitPrice,QuantityPerUnit")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction("Index");
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                          .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'WarehouseContext.Product'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
        //[HttpPost]
        //public async Task<IActionResult> CreateProduct([FromBody] Product product)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    _context.Products.Add(product);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        //}
    }
}
