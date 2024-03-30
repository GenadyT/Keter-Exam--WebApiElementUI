using System.ComponentModel.DataAnnotations;

namespace WebApiElementUI.Models
{
    public class ProductInput
    {
        [Key]
        public int? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? SalesStartDate { get; set; }        
        public IFormFile Image { get; set; }
    }
}
