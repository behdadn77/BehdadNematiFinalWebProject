using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BehdadNematiFinalWebProject.Models
{
    public class purchaseCart_product
    {
        public int Id { get; set; }
        public int product_Id { get; set; }
        [ForeignKey("product_Id")]
        public product product { get; set; }
        public int purchaseCart_Id { get; set; }
        [ForeignKey("purchaseCart_Id")]
        public purchaseCart purchaseCart { get; set; }
        public int count { get; set; }

    }
}
