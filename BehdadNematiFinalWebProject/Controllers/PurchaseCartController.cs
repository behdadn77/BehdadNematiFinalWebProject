using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BehdadNematiFinalWebProject.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using BehdadNematiFinalWebProject.Models;
using Microsoft.AspNetCore.Authorization;

namespace BehdadNematiFinalWebProject.Controllers
{
    public class PurchaseCartController : Controller
    {
        UserManager<ApplicationUser> userManager;
        ApplicationContext db;
        public PurchaseCartController(UserManager<ApplicationUser> _userManager, ApplicationContext _db)
        {
            userManager = _userManager;
            db = _db;
        }
        public async Task<IActionResult> ShowPurchaseCart()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            if (user != null)
            {
                var Purchscart = db.PurchaseCarts.Where(x => x.user_Id == user.Id && x.isPaid == false).LastOrDefault();
                if (Purchscart != null)
                {
                                    ViewData["totalprice"] =
                    (await GetPurchaseCartProductTotalPriceAsync()).ToString("C").Replace("/00", "");//currency

                    var PurchaseCartProducts = db.PurchaseCart_Products.Where(x => x.PurchaseCart_Id == Purchscart.Id);
                    return View(PurchaseCartProducts.Include(x => x.Product).Include(x => x.Product.images).ToList());
                }
                return View("Purchase cart not found!");

            }
            return RedirectToAction("login", "Account");
        }
        //----------------------------------------------------------------------//
        [Authorize]
        public async Task<IActionResult> AddItemToPurchaseCartAsync(int Id)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            if (user != null)
            {
                var Purchscart = db.PurchaseCarts.Where(x => x.user_Id == user.Id && x.isPaid == false).FirstOrDefault();
                if (Purchscart == null)
                {
                    Purchscart = new PurchaseCart { user_Id = user.Id, isPaid = false, pDate = DateTime.Now };
                    db.Add(Purchscart);
                    db.SaveChanges();
                }
                var PurchaseCartProduct = new PurchaseCart_Product { PurchaseCart_Id = Purchscart.Id, count = 1, Product_Id = Id };
                db.Add(PurchaseCartProduct);
                db.SaveChanges();
                return Json(true);
            }
            return Json(false);
        }
        [Authorize]
        public async Task<IActionResult> RemoveItemFromPurchaseCart(int Id)
        {
            try
            {
                PurchaseCart_Product prchsCartPrduct = db.PurchaseCart_Products.Find(Id);
                db.Remove(prchsCartPrduct);
                db.SaveChanges();
                return Json(new { count= await GetPurchaseCartProductCountAsync(),
                                  totalPrice = await GetPurchaseCartProductTotalPriceAsync() });
            }
            catch
            {
                return Json(false);
            }
        }
        //----------------------------------------------------------------------//
        [Authorize]
        public async Task<int> GetPurchaseCartProductCountAsync()
        {
            int count = 0;
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            if (user != null)
            {
                var PurchaseCart = db.PurchaseCarts.Where(x => x.user_Id == user.Id && !x.isPaid).FirstOrDefault();
                if (PurchaseCart != null)
                {
                    var p = db.PurchaseCart_Products.Where(x => x.PurchaseCart_Id == PurchaseCart.Id).ToList();
                    count = p.Count();
                    //p.Sum(x=>x.count);
                    //p.ForEach(x => {
                    //    count += x.count;
                    //});
                }
            }
            return count;
        }
        [Authorize]
        public async Task<int> GetPurchaseCartProductTotalPriceAsync()
        {
            int totalPrice = 0;
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            if (user != null)
            {
                var PurchaseCart = db.PurchaseCarts.Where(x => x.user_Id == user.Id && !x.isPaid).FirstOrDefault();
                if (PurchaseCart != null)
                {
                    var p = db.PurchaseCart_Products.Where(x => x.PurchaseCart_Id == PurchaseCart.Id).Include(x=>x.Product).ToList();
                    totalPrice = p.Sum(x=>x.count*x.Product.price);
                }
            }
            return totalPrice;
        }
        //----------------------------------------------------------------------//
        [Authorize]
        public async Task<IActionResult> IncreasePrdtCountInPurchaseCartAsync(int Id)
        {
            var prdt = db.PurchaseCart_Products.Find(Id);
            if (prdt !=null)
            {
            int AvailablePrdtCount= db.Products.Find(prdt.Product_Id).count;
                if ((prdt.count)+1 <= AvailablePrdtCount)
                {
                    prdt.count++;
                    db.SaveChanges();
                    return Json(new {
                        totalPrice = await GetPurchaseCartProductTotalPriceAsync(),
                       // count = await GetPurchaseCartProductCountAsync(),
                        ProductCount = prdt.count
                    });
                }
                else
                {
                    return Json(false);
                }
            }
            return View("Somthing went wrong!");
        }
        [Authorize]
        public async Task<IActionResult> DecreasePrdtCountInPurchaseCartAsync(int Id)
        {
            var prdt = db.PurchaseCart_Products.Find(Id);
            if (prdt != null)
            {
                int AvailablePrdtCount = db.Products.Find(prdt.Product_Id).count;
                if (prdt.count >1)
                {
                    prdt.count--;
                    db.SaveChanges();
                    return Json(new
                    {
                        totalPrice = await GetPurchaseCartProductTotalPriceAsync(),
                        //count = await GetPurchaseCartProductCountAsync(),
                        ProductCount = prdt.count
                    });
                }
                else
                {
                    return Json(false);
                }
            }
            return View("Somthing went wrong!");
        }


    }

}