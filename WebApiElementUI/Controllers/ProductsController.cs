using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiElementUI.Models;

namespace WebApiElementUI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductContext _context;

        public ProductsController(ProductContext context)
        {
            _context = context;
        }

        [EnableCors("WelcomePolicy")]
        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        [EnableCors("WelcomePolicy")]
        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [EnableCors("WelcomePolicy")]
        // GET: api/Product image
        //[Route("GetProductImage")]
        [HttpGet("GetProductImage/{image}")]
        public async Task<ActionResult<Product>> GetProductImage(string image)
        {
            string imagePath = "ProductImages/" + image;

            if (System.IO.File.Exists(imagePath))
            {
                return File(System.IO.File.OpenRead(imagePath), "application/octet-stream", Path.GetFileName(imagePath));
            }
            return NotFound();

            ////var product = await _context.Products.FindAsync(id);
            ////var product = new ActionResult<Product>()
            //Product product = null;

            //if (product == null)
            //{
            //    return NotFound();
            //}

            //return product;
        }

        [EnableCors("WelcomePolicy")]
        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{code:int}")]
        public async Task<IActionResult> PutProduct(int code, Product product)
        {
            if (code != product.Code)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ProductExists(code))
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

        [EnableCors("WelcomePolicy")]
        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            try
            {
                product.Code = null;
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                //return CreatedAtAction("GetProduct", new { id = product.Code }, product);
                return Ok(product.Code);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [EnableCors("WelcomePolicy")]
        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok();    // NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Code == id);
        }
    }
}
