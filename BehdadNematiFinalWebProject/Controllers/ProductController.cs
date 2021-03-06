﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Areas.Identity.Data;
using Shop.Models;
using Shop.Models.ViewModels;

namespace Shop.Controllers
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

        public async Task<IActionResult> TopProductsItems()
        {
            //var Product = db.Products.Include(x => x.images).ToList();
            var Products = db.TopProducts.Include(x => x.Product).ToList();
            List<PurchaseCart_Product> userPurchCartPrdtProductLst = new List<PurchaseCart_Product>();
            if (User.Identity.Name != null)
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                if (user != null)
                {
                    var userPurchCart = db.PurchaseCarts.Where(x => x.User_Id == user.Id && x.IsPaid == false).ToList();
                    if (userPurchCart.Count > 0)
                    {
                        userPurchCartPrdtProductLst = db.PurchaseCart_Products.Where(x => x.PurchaseCart_Id == userPurchCart.Last().Id).ToList();
                    }
                }
            }
            List<ShowProductViewModel> ProductLst = new List<ShowProductViewModel>();
            foreach (var item in Products)
            {
                ShowProductViewModel p = new ShowProductViewModel()
                {
                    Id = item.Id,
                    EnglishName = item.Product.EnglishName,
                    Count = item.Product.Count,
                    Price = item.Product.Price,
                    IsAproved = item.Product.IsAproved,
                    ProductType_Id = item.Product.ProductType_Id,
                    Brand_Id = item.Product.Brand_Id,
                    ThumbnailImage = Convert.ToBase64String(item.Product.ThumbnailImage),
                    Image = Convert.ToBase64String(item.Product.Image),
                    SelectedInCart = false
                };

                if (userPurchCartPrdtProductLst.Where(x => x.Product_Id == item.Id) != null)
                {
                    if (userPurchCartPrdtProductLst.Where(x => x.Product_Id == item.Id).ToList().Count > 0)
                    {
                        p.SelectedInCart = true;
                    }
                }
                ProductLst.Add(p);
            }
            return PartialView(ProductLst);
        }

        //------------------------------------------------
        public IActionResult ShowProducts()
        {
            return View();
        }

        public async Task<IActionResult> ProductItems()
        {
            var Products = db.Products.ToList();
            List<PurchaseCart_Product> userPurchCartPrdtProductLst = new List<PurchaseCart_Product>();
            if (User.Identity.Name != null)
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                if (user != null)
                {
                    var userPurchCart = db.PurchaseCarts.Where(x => x.User_Id == user.Id && x.IsPaid == false).ToList();
                    if (userPurchCart.Count > 0)
                    {
                        userPurchCartPrdtProductLst = db.PurchaseCart_Products.Where(x => x.PurchaseCart_Id == userPurchCart.Last().Id).ToList();
                    }
                }
            }
            List<ShowProductViewModel> ProductLst = new List<ShowProductViewModel>();
            foreach (var item in Products)
            {
                ShowProductViewModel p = new ShowProductViewModel()
                {
                    Id = item.Id,
                    EnglishName = item.EnglishName,
                    Count = item.Count,
                    Price = item.Price,
                    IsAproved = item.IsAproved,
                    ProductType_Id = item.ProductType_Id,
                    Brand_Id = item.Brand_Id,
                    ThumbnailImage = item.ThumbnailImage != null ? Convert.ToBase64String(item.ThumbnailImage) : null,
                    Image = item.Image != null ? Convert.ToBase64String(item.Image) : null,
                    SelectedInCart = false
                };

                if (userPurchCartPrdtProductLst.Where(x => x.Product_Id == item.Id) != null)
                {
                    if (userPurchCartPrdtProductLst.Where(x => x.Product_Id == item.Id).ToList().Count > 0)
                    {
                        p.SelectedInCart = true;
                    }
                }
                ProductLst.Add(p);
            }
            return PartialView(ProductLst);
        }

    }
}