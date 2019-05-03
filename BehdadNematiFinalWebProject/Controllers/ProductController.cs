using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BehdadNematiFinalWebProject.Areas.Identity.Data;
using BehdadNematiFinalWebProject.Models;
using BehdadNematiFinalWebProject.Models.ViewModels;

namespace BehdadNematiFinalWebProject.Controllers
{
    public class ProductController : Controller
    {
        ApplicationContext db;
        UserManager<ApplicationUser> userManager;
        public ProductController(ApplicationContext _db, UserManager<ApplicationUser> _userManager)
        {
            db = _db;
            userManager = _userManager;
        }
        
        public IActionResult showProducts()
        {
            return View();
        }
        public async Task<IActionResult> ProductItems()
        {
            var Product = db.Products.Include(x => x.images).ToList();
            List<PurchaseCart_Product> userPurchCartPrdtProductLst = new List<PurchaseCart_Product>();
            if (User.Identity.Name!=null)
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                if (user != null)
                {
                    var userPurchCart = db.PurchaseCarts.Where(x => x.user_Id == user.Id && x.isPaid == false).ToList();
                    if (userPurchCart.Count>0)
                    {
                         userPurchCartPrdtProductLst = db.PurchaseCart_Products.Where(x => x.PurchaseCart_Id == userPurchCart.Last().Id).ToList();
                    }
                }
            }
            List<ShowProductViewModel> ProductLst = new List<ShowProductViewModel>();
            foreach (var item in Product)
            {
                ShowProductViewModel p = new ShowProductViewModel()
                {
                    Id = item.Id,
                    EnglishName = item.EnglishName,
                    count = item.count,
                    price = item.price,
                    isAproved = item.isAproved,
                    ProductType_Id = item.ProductType_Id,
                    Brand_Id = item.Brand_Id,
                    images=item.images,
                    SelectedInCart=false
                };
                
                if (userPurchCartPrdtProductLst.Where(x => x.Product_Id == item.Id)!=null)
                {
                    if (userPurchCartPrdtProductLst.Where(x => x.Product_Id == item.Id).ToList().Count>0)
                    {
                        p.SelectedInCart = true;    
                    }
                }
                ProductLst.Add(p);
            }
            return PartialView(ProductLst);
        }
        //public async Task<IActionResult> checkPrdtInPurchsCart(int ProductId)
        //{
        //    var user = await userManager.FindByNameAsync(User.Identity.Name);
        //    if (user != null)
        //    {
        //        var userPurchCart = db.PurchaseCarts.Single(x => x.user_Id == user.Id && x.isPaid == false);
        //        if (userPurchCart != null)
        //        {
        //            var userPurchCartPrdtProductLst = db.PurchaseCart_Products.Where(x => x.PurchaseCart_Id == userPurchCart.Id).ToList();
        //            var st = userPurchCartPrdtProductLst.Where(x => x.Product_Id == ProductId).ToList();
        //            if (st != null)
        //            {
        //                return Json(true);
        //            }

        //        }
        //    }
        //    return Json(false);
        //}

    }
}