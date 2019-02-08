using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BehdadNematiFinalWebProject.Models
{
    public class product
    {
        public int Id { get; set; }
        public string EnglishName { get; set; }
        public int count { get; set; }
        public int price { get; set; }
        public bool isAproved { get; set; }
        public int productType_Id { get; set; }
        [ForeignKey("productType_Id")]
        public ProductType productType { get; set; }
        public int brand_Id { get; set; }
        [ForeignKey("brand_Id")]
        public brand brand { get; set; }

        public ICollection<topProduct> topProducts { get; set; }
        public ICollection<specialOffers> specialOffers { get; set; }
        public ICollection<purchaseCart_product> purchaseCart_Products { get; set; }
        public ICollection<image> images { get; set; }
    }
}
