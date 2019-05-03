using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using BehdadNematiFinalWebProject.Areas.Identity.Data;

namespace BehdadNematiFinalWebProject.Models
{
    public class PurchaseCart
    {
        public int Id { get; set; }
        public string user_Id { get; set; }
        [ForeignKey("user_Id")]
        public ApplicationUser user { get; set; }
        public bool isPaid { get; set; } = false;
        public DateTime pDate { get; set; }
        public DateTime exDate { get; set; }
    }
}
