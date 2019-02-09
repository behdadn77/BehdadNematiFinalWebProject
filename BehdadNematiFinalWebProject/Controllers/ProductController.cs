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
        [Authorize]
        public IActionResult insert()
        {
            ViewData["Brand"] = db.brands.ToList();
            ViewData["productType"] = db.ProductTypes.ToList();
            return View();
        }
        [Authorize]
        public async Task<IActionResult> insertComfirm(ProductViewModel model , IFormFile img)
        {
            product objprd = new product
            {
                EnglishName = model.EnglishName,
                count = model.count,
                price = model.price,
                brand_Id = model.brand_Id,
                isAproved = true,
                productType_Id = model.productType_Id
            };
            image objimg = new image();
            var id = db.products.ToList();
            if (img != null)
            {
                byte[] image = new byte[img.Length];
                img.OpenReadStream().Read(image, 0, image.Length);
                objimg.img = image;
                int lastid = 0;
                foreach (var item in id)
                {
                     lastid =item.Id;
                }
                objimg.product_Id = lastid; 
            }
            db.Add(objprd);
            db.Add(objimg);
            db.SaveChanges();
            return RedirectToAction("insert");
        }
        [Authorize]
        public IActionResult TopProducts()
        {
            var emp = db.topProducts.Include(x => x.product).Include(x => x.product.images).ToList();
            return PartialView(emp);
        }
        [Authorize]
        public IActionResult SpecialOffers()
        {
            return PartialView();
        }
        [Authorize]
        public IActionResult insertImage()
        {
            ViewData["product"] = db.products.ToList();
            return View();
        }
        public async Task<IActionResult> insertImageComfirm(imageViewModels model)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            image objimg = new image
            {
               // product_Id = model.product_Id
            };
            if (model.img != null)
            {
                byte[] image = new byte[model.img.Length];
                model.img.OpenReadStream().Read(image, 0, image.Length);
                objimg.img = image;
            }
            db.Add(objimg);
            db.SaveChanges();
            return RedirectToAction("insert");
        }
        [Authorize]
        public IActionResult insertBrand()
        {
            return View();
        }
        public IActionResult insertBrandConfirm(brandViewModels models)
        {
            brand objbrand = new brand()
            {
                name=models.name
            };
            db.Add(objbrand);
            db.SaveChanges();
            return RedirectToAction("insert");
        }
        //public IActionResult insertTopProducts()
        //{

        //}
        //[Authorize]
        //public IActionResult insertSpecialOffers()
        //{

        //}
        
        public IActionResult showProduct()
        {
            //return View(db.products.Include(x => x.images).ToList());
            return View();
        }
        public async Task<IActionResult> ProductItems()
        {
            var product = db.products.Include(x => x.images).ToList();
            List<purchaseCart_product> userPurchCartPrdtLst = new List<purchaseCart_product>();
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            if (user != null)
            {
                var userPurchCart = db.purchaseCarts.Where(x => x.user_Id == user.Id && x.isPaid == false).ToList();
                if (userPurchCart != null)
                {
                    foreach (var item in userPurchCart)
                    {
                     userPurchCartPrdtLst = db.purchaseCart_Products.Where(x => x.purchaseCart_Id == item.Id).ToList();
                    }
                }
            }
            List<ShowProductViewModel> lst = new List<ShowProductViewModel>();
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
                
                if (userPurchCartPrdtLst.Where(x => x.product_Id == item.Id)!=null)
                {
                    if (userPurchCartPrdtLst.Where(x => x.product_Id == item.Id).ToList().Count>0)
                    {

                        p.SelectedInCart = true;    
                    }
                }
                lst.Add(p);
            }
            return PartialView(lst);
        }
        public async Task<IActionResult> checkPrdtInPurchsCart(int productId)
        {

            var user = await userManager.FindByNameAsync(User.Identity.Name);
            if (user != null)
            {
                var userPurchCart = db.purchaseCarts.Single(x => x.user_Id == user.Id && x.isPaid == false);
                if (userPurchCart != null)
                {
                    var userPurchCartPrdtLst = db.purchaseCart_Products.Where(x => x.purchaseCart_Id == userPurchCart.Id).ToList();
                    var st = userPurchCartPrdtLst.Where(x => x.product_Id == productId).ToList();
                    if (st != null)
                    {
                        return Json(true);
                    }

                }
            }
            return Json(false);
        }

    }
}