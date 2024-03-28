using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiElementUI.Models
{
    [Table("tblProducts")]
    public class Product
    {
        [Key]
        public int? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? SalesStartDate { get; set; }
        public string? Image { get; set; }
    }
}
