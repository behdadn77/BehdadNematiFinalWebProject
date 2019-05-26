using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Shop.Areas.Identity.Data;

namespace Shop.Models
{
    public class PurchaseCart
    {
        public int Id { get; set; }
        public string User_Id { get; set; }
        [ForeignKey("User_Id")]
        public ApplicationUser User { get; set; }
        public bool IsPaid { get; set; } = false;
        public DateTime PDate { get; set; }
        public DateTime ExDate { get; set; }
    }
}
