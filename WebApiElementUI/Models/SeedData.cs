using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
//using MvcMovie.Data;
using WebApiElementUI.Models;
using System;
using System.Linq;

namespace WebApiElementUI.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ProductContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<ProductContext>>()))
        {
            // Look for any movies.
            if (context.Products.Any())
            {
                return;   // DB has been seeded
            }
            /*context.Movie.AddRange(
                new Product
                {
                    
                }, ...
            );
            context.SaveChanges();*/
        }
    }
}