using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.ViewModels
{
    public class ShowProductViewModel
    {
        public int Id { get; set; }
        public string EnglishName { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
        public bool IsAproved { get; set; }
        public int ProductType_Id { get; set; }
        
        public ProductType ProductType { get; set; }
        public int Brand_Id { get; set; }
        
        public Brand Brand { get; set; }

        public ICollection<TopProducts> TopProducts { get; set; }
        public ICollection<SpecialOffers> SpecialOffers { get; set; }
        public ICollection<PurchaseCart_Product> PurchaseCart_Products { get; set; }
        //public ICollection<image> Images { get; set; }
        public bool SelectedInCart { get; set; }
        public string ThumbnailImage { get; set; }
        public string Image { get; set; }
    }
}
