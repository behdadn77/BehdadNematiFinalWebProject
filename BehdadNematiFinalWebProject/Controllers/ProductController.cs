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
            var product = db.products.Include(x => x.images).ToList();
            List<purchaseCart_product> userPurchCartPrdtProductLst = new List<purchaseCart_product>();
            if (User.Identity.Name!=null)
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                if (user != null)
                {
                    var userPurchCart = db.purchaseCarts.Where(x => x.user_Id == user.Id && x.isPaid == false).ToList();
                    if (userPurchCart.Count>0)
                    {
                         userPurchCartPrdtProductLst = db.purchaseCart_Products.Where(x => x.purchaseCart_Id == userPurchCart.Last().Id).ToList();
                    }
                }
            }
            List<ShowProductViewModel> ProductLst = new List<ShowProductViewModel>();
            foreach (var item in product)
            {
                ShowProductViewModel p = new ShowProductViewModel()
                {
                    Id = item.Id,
                    EnglishName = item.EnglishName,
                    count = item.count,
                    price = item.price,
                    isAproved = item.isAproved,
                    productType_Id = item.productType_Id,
                    brand_Id = item.brand_Id,
                    images=item.images,
                    SelectedInCart=false
                };
                
                if (userPurchCartPrdtProductLst.Where(x => x.product_Id == item.Id)!=null)
                {
                    if (userPurchCartPrdtProductLst.Where(x => x.product_Id == item.Id).ToList().Count>0)
                    {
                        p.SelectedInCart = true;    
                    }
                }
                ProductLst.Add(p);
            }
            return PartialView(ProductLst);
        }
        //public async Task<IActionResult> checkPrdtInPurchsCart(int productId)
        //{
        //    var user = await userManager.FindByNameAsync(User.Identity.Name);
        //    if (user != null)
        //    {
        //        var userPurchCart = db.purchaseCarts.Single(x => x.user_Id == user.Id && x.isPaid == false);
        //        if (userPurchCart != null)
        //        {
        //            var userPurchCartPrdtProductLst = db.purchaseCart_Products.Where(x => x.purchaseCart_Id == userPurchCart.Id).ToList();
        //            var st = userPurchCartPrdtProductLst.Where(x => x.product_Id == productId).ToList();
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