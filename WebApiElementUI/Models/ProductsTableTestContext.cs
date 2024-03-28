using Microsoft.EntityFrameworkCore;

namespace WebApiElementUI.Models
{
    public class ProductsTableTestContext : DbContext
    {
        public ProductsTableTestContext(DbContextOptions<ProductContext> options)
        : base(options)
        {

        }

        public DbSet<Product> Products => Set<Product>();
    }
}
