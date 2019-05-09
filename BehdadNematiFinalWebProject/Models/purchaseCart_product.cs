using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BehdadNematiFinalWebProject.Models
{
    public class PurchaseCart_Product
    {
        public int Id { get; set; }
        public int Product_Id { get; set; }
        [ForeignKey("Product_Id")]
        public Product Product { get; set; }
        public int PurchaseCart_Id { get; set; }
        [ForeignKey("PurchaseCart_Id")]
        public PurchaseCart PurchaseCart { get; set; }
        public int Count { get; set; }

    }
}
