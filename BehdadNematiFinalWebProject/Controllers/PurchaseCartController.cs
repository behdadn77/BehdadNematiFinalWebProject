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
        public async Task<IActionResult> addToPrchusCrt(int productId)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            if (user != null)
            {
                var Purchscart = db.purchaseCarts.Where(x => x.user_Id == user.Id && x.isPaid == false).FirstOrDefault();
                if (Purchscart == null)
                {
                    Purchscart = new purchaseCart { user_Id = user.Id, isPaid = false, pDate = DateTime.Now };
                    db.Add(Purchscart);
                    db.SaveChanges();
                }
                var purchaseCartproduct = new purchaseCart_product { purchaseCart_Id = Purchscart.Id, count = 1, product_Id = productId };
                db.Add(purchaseCartproduct);
                db.SaveChanges();
                return Json(true);
            }
            return Json(false);
        }
        [Authorize]
        public async Task<IActionResult> showPurchaseCart()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            if (user != null)
            {
                var Purchscart = db.purchaseCarts.Where(x => x.user_Id == user.Id && x.isPaid == false).FirstOrDefault();
                if (Purchscart != null)
                {
                    var purchaseCartProducts = db.purchaseCart_Products.Where(x => x.purchaseCart_Id == Purchscart.Id);

                    return View(purchaseCartProducts.Include(x => x.product).Include(x => x.product.images).ToList());
                }
                return View("Purchase cart not found!");

            }
            return RedirectToAction("login", "Account");
        }
        public async Task<IActionResult> removeFromCart(int prchsCartPrductId)
        {
            try
            {
                purchaseCart_product prchsCartPrduct = db.purchaseCart_Products.Find(prchsCartPrductId);
                db.Remove(prchsCartPrduct);
                db.SaveChanges();
                return Json(new { count= await getPurchaseCartProductCountAsync(),
                                  totalPrice = await getPurchaseCartProductTotalPriceAsync() });
            }
            catch
            {
                return Json(false);
            }
        }
        public async Task<int> getPurchaseCartProductCountAsync()
        {
            int count = 0;
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            if (user != null)
            {
                var purchaseCart = db.purchaseCarts.Where(x => x.user_Id == user.Id && !x.isPaid).FirstOrDefault();
                if (purchaseCart != null)
                {
                    var p = db.purchaseCart_Products.Where(x => x.purchaseCart_Id == purchaseCart.Id).ToList();
                    count = p.Count();
                    //p.Sum(x=>x.count);
                    //p.ForEach(x => {
                    //    count += x.count;
                    //});
                }
            }
            return count;
        }
        public async Task<int> getPurchaseCartProductTotalPriceAsync()
        {
            int totalPrice = 0;
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            if (user != null)
            {
                var purchaseCart = db.purchaseCarts.Where(x => x.user_Id == user.Id && !x.isPaid).FirstOrDefault();
                if (purchaseCart != null)
                {
                    var p = db.purchaseCart_Products.Where(x => x.purchaseCart_Id == purchaseCart.Id).Include(x=>x.product).ToList();
                    totalPrice = p.Sum(x=>x.count*x.product.price);
                    //p.ForEach(x => {
                    //    totalPrice += x.product.price* x.count;
                    //});
                }
            }
            return totalPrice;
        }
        public async Task<IActionResult> increasePrdtCountPurchaseCartAsync(int prchsCartPrductId)
        {
            var prdt = db.purchaseCart_Products.Find(prchsCartPrductId);
            if (prdt !=null)
            {
            int isAvailablePrdt= db.products.Find(prdt.product_Id).count;
                if ((prdt.count)+1 <= isAvailablePrdt)
                {
                    prdt.count++;
                    db.SaveChanges();
                    return Json(new {
                        totalPrice = await getPurchaseCartProductTotalPriceAsync(),
                        count = await getPurchaseCartProductCountAsync(),
                        productCount = prdt.count
                    });
                }
                else
                {
                    return Json(false);
                }
            }
            return RedirectToAction("Product","showProduct");
        }
        public async Task<IActionResult> decreasePrdtCountPurchaseCart(int prchsCartPrductId)
        {
            var prdt = db.purchaseCart_Products.Find(prchsCartPrductId);
            if (prdt != null)
            {
                int isAvailablePrdt = db.products.Find(prdt.product_Id).count;
                if (prdt.count >1)
                {
                    prdt.count--;
                    db.SaveChanges();
                    return Json(new
                    {
                        totalPrice = await getPurchaseCartProductTotalPriceAsync(),
                        count = await getPurchaseCartProductCountAsync(),
                        productCount = prdt.count
                    });
                }
                else
                {
                    return Json(false);
                }
            }
            return RedirectToAction("Product", "showProduct");
        }


    }

}