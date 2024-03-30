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
        // GET: api/GetProductImage/5
        [HttpGet("GetProductImage/{code}")]
        public async Task<ActionResult<MemoryStream>> GetProductImage(int code)
        {
            var product = await _context.Products.FindAsync(code);

            if (product == null)
            {
                return NotFound();
            }
            
            MemoryStream stream = new MemoryStream(product.Image);            
            return Ok(stream);
        }        

        [EnableCors("WelcomePolicy")]
        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{code:int}")]
        public async Task<IActionResult> PutProduct(int code, [FromForm] ProductInput productInput)
        {
            if (code != productInput.Code)
            {
                return BadRequest();
            }

            Product product = null;
            saveChangesPrepare(productInput, ref product, _context);

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

        private void saveChangesPrepare(ProductInput productInput, ref Product product, ProductContext context)
        {            
            bool dummiedImage = false;
            if (((Microsoft.AspNetCore.Http.FormFile)productInput.Image).FileName.Contains("dummyFile"))
            {
                dummiedImage = true;                
            }

            product = ByInputProduct(productInput, dummiedImage);

            var entry = context.Entry(product);
            entry.State = EntityState.Modified;
            if (dummiedImage)
            {
                entry.Property("Image").IsModified = false;
            }
        }

        [EnableCors("WelcomePolicy")]
        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromForm] ProductInput productInput)
        {
            Product product = null;
            saveChangesPrepare(productInput, ref product, _context);

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

        private Product ByInputProduct(ProductInput productInput, bool dummiedImage)
        {
            Product product = new Product();
            product.Code = productInput.Code;
            product.Name = productInput.Name;
            product.Description = productInput.Description;
            product.SalesStartDate = productInput.SalesStartDate;

            if(!dummiedImage)
            {
                using var fileStream = productInput.Image.OpenReadStream();
                long length = productInput.Image.Length;
                byte[] bytes = new byte[length];
                fileStream.Read(bytes, 0, (int)productInput.Image.Length);
                product.Image = bytes;
            }

            return product;
        }
    }
}
