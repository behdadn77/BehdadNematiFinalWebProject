using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BehdadNematiFinalWebProject.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string EnglishName { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
        public bool IsAproved { get; set; }
        public int ProductType_Id { get; set; }
        [ForeignKey("ProductType_Id")]
        public ProductType ProductType { get; set; }
        public int Brand_Id { get; set; }
        [ForeignKey("Brand_Id")]
        public Brand Brand { get; set; }

        public ICollection<TopProducts> TopProducts { get; set; }
        public ICollection<SpecialOffers> SpecialOffers { get; set; }
        public ICollection<PurchaseCart_Product> PurchaseCart_Products { get; set; }
        public ICollection<image> images { get; set; }
    }
}
