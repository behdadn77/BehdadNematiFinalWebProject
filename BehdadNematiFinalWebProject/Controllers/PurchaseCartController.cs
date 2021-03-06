﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Shop.Models;
using Microsoft.AspNetCore.Authorization;

namespace Shop.Controllers
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
        [Authorize]
        public async Task<IActionResult> ShowPurchaseCart()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            if (user != null)
            {
                var Purchscart = db.PurchaseCarts.Where(x => x.User_Id == user.Id && x.IsPaid == false).LastOrDefault();
                if (Purchscart != null)
                {
                                    ViewData["totalprice"] =
                    (await GetPurchaseCartProductTotalPriceAsync()).ToString("C").Replace("/00", "");//currency

                    var PurchaseCartProducts = db.PurchaseCart_Products.Where(x => x.PurchaseCart_Id == Purchscart.Id);
                    return View(PurchaseCartProducts.Include(x => x.Product).ToList());
                }
                return Json("No Items");

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
                var Purchscart = db.PurchaseCarts.Where(x => x.User_Id == user.Id && x.IsPaid == false).FirstOrDefault();
                if (Purchscart == null)
                {
                    Purchscart = new PurchaseCart { User_Id = user.Id, IsPaid = false, PDate = DateTime.Now };
                    db.Add(Purchscart);
                    db.SaveChanges();
                }
                var PurchaseCartProduct = new PurchaseCart_Product { PurchaseCart_Id = Purchscart.Id, Count = 1, Product_Id = Id };
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
                var PurchaseCart = db.PurchaseCarts.Where(x => x.User_Id == user.Id && !x.IsPaid).FirstOrDefault();
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
                var PurchaseCart = db.PurchaseCarts.Where(x => x.User_Id == user.Id && !x.IsPaid).FirstOrDefault();
                if (PurchaseCart != null)
                {
                    var p = db.PurchaseCart_Products.Where(x => x.PurchaseCart_Id == PurchaseCart.Id).Include(x=>x.Product).ToList();
                    totalPrice = p.Sum(x=>x.Count*x.Product.Price);
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
            int AvailablePrdtCount= db.Products.Find(prdt.Product_Id).Count;
                if ((prdt.Count)+1 <= AvailablePrdtCount)
                {
                    prdt.Count++;
                    db.SaveChanges();
                    return Json(new {
                        totalPrice = await GetPurchaseCartProductTotalPriceAsync(),
                       // count = await GetPurchaseCartProductCountAsync(),
                        ProductCount = prdt.Count
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
                int AvailablePrdtCount = db.Products.Find(prdt.Product_Id).Count;
                if (prdt.Count >1)
                {
                    prdt.Count--;
                    db.SaveChanges();
                    return Json(new
                    {
                        totalPrice = await GetPurchaseCartProductTotalPriceAsync(),
                        //count = await GetPurchaseCartProductCountAsync(),
                        ProductCount = prdt.Count
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