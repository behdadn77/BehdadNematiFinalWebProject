using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class TopProducts
    {
        public int Id { get; set; }
        public int Product_Id { get; set; }
        [ForeignKey("Product_Id")]
        public Product Product { get; set; }
    }
}
