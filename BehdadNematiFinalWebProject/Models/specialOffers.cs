using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BehdadNematiFinalWebProject.Models
{
    public class specialOffers
    {
        public int Id { get; set; }
        public int product_Id { get; set; }
        [ForeignKey("product_Id")]
        public product product { get; set; }

    }
}
