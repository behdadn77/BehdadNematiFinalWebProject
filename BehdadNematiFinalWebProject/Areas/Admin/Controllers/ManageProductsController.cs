using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Shop.Areas.Identity.Data;
using System.Drawing;
using System.IO;

namespace Shop.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admins")]
    public class ManageProductsController : Controller
    {
        ApplicationContext db;

        UserManager<ApplicationUser> userManager;

        public ManageProductsController(ApplicationContext _db, UserManager<ApplicationUser> _userManager)
        {
            db = _db;
            userManager = _userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProductTypeComboItems()
        {
            return PartialView(db.ProductTypes.ToList());
        }

        public IActionResult ProductBrandComboItems()
        {
            return PartialView(db.Brands.ToList());
        }

        //---------Add Product---------//
        public IActionResult AddProduct()
        {
            return View();
        }

        public IActionResult AddProductConfirm(ProductViewModel model)
        {
            Product p = new Product()
            {
                EnglishName = model.EnglishName,
                Price = model.Price,
                Count = model.Count,
                ProductType_Id = model.ProductType_Id,
                Brand_Id = model.Brand_Id
            };
            if (model.Pictures.Count > 0)
            {
                byte[] b = new byte[model.Pictures.FirstOrDefault().Length];
                model.Pictures.FirstOrDefault().OpenReadStream().Read(b, 0, b.Length);
                ImageThumbnailMaker imageThumbnailMaker = new ImageThumbnailMaker();
                p.ThumbnailImage = imageThumbnailMaker.CreateThumbNail(Image.FromStream(new MemoryStream(b)));
                p.Image = b;
            }
            db.Add(p);
            if (db.SaveChanges() != 0)
            {
                return RedirectToAction("Productlist");
            }
            return Json("Error!");
        }

        //-------Edit Product-------//
        public IActionResult EditProduct(int id)
        {
            Product p = db.Products.Find(id);
            if (p != null)
            {
                ProductViewModel ProductViewModel = new ProductViewModel()
                {
                    id = p.Id,
                    EnglishName = p.EnglishName,
                    Count = p.Count,
                    Price = p.Price,
                    Brand_Id = p.Brand_Id,
                    ProductType_Id = p.ProductType_Id,
                    ImageBase64 = $"data:image;base64,{Convert.ToBase64String(p.Image)}",
                    thumbnailImageBase64 = $"data:image;base64,{Convert.ToBase64String(p.ThumbnailImage)}"
                };
                return View(ProductViewModel);

            }
            return Json("Product not found!");
        }
        public IActionResult EditProductConfirm(ProductViewModel model)
        {
            Product p = db.Products.Find(model.id);
            p.EnglishName = model.EnglishName;
            p.Brand_Id = model.Brand_Id;
            p.ProductType_Id = model.ProductType_Id;
            p.Count = model.Count;
            p.Price = model.Price;
            if (model.Pictures.Count > 0)
            {
                byte[] b = new byte[model.Pictures.FirstOrDefault().Length];
                model.Pictures.FirstOrDefault().OpenReadStream().Read(b, 0, b.Length);
                ImageThumbnailMaker imageThumbnailMaker = new ImageThumbnailMaker();
                p.ThumbnailImage = imageThumbnailMaker.CreateThumbNail(Image.FromStream(new MemoryStream(b)));
                p.Image = b;
            }
            if (db.SaveChanges() != 0)
            {
                return RedirectToAction("ProductList");
            }
            return View("Error!");
        }
        //-------Delete Product-----//
        public IActionResult DeleteProduct(int id)
        {
            Product p = db.Products.Find(id);
            if (p != null)
            {
                db.Remove(p);
                if (db.SaveChanges() != 0)
                {
                    return RedirectToAction("Productlist");
                }
                return View("Error");
            }
            return View("Product not found");
        }
        //------------Product List-------------//
        public IActionResult ProductList()
        {
            return View();
        }

        public IActionResult ProductListItems(string Search, int TypeId = 0, int BrandId = 0, int Page = 1)
        {
            List<Product> p = new List<Product>();
            if (Search == null)
            {
                p = FindProductByBrandType(TypeId, BrandId).ToList();
            }
            else
            {
                p = FindProductByBrandType(TypeId, BrandId).Where(x => x.EnglishName.Contains(Search)
                           ).ToList();
            }

            --Page; //array starts at 0 :D
            Page *= 10;
            // return PartialView(p.GetRange(Page,(Page+10<p.Count?Page+10: p.Count)));
            return PartialView(p); // need to add infinit scroll with ajax later !!!
        }
        public List<Product> FindProductByBrandType(int TypeId, int BrandId)
        {
            List<Product> p = db.Products.Include(x => x.Brand).Include(x => x.ProductType).ToList();
            if (TypeId == 0 && BrandId == 0) //Brand and type are not specified
            {
                return p;
            }
            else if (TypeId != 0 && BrandId == 0) //Brand is not specified
            {
                return p.Where(x => x.ProductType_Id == TypeId).ToList();
            }
            else if (BrandId != 0 && TypeId == 0) //Type is not specified
            {
                return p.Where(x => x.Brand_Id == BrandId).ToList();
            }
            else //both Brand and type are selected
            {
                return p.Where(x => x.Brand_Id == BrandId && x.ProductType_Id == TypeId).ToList();
            }

        }
        //------select Top Products----//
        public IActionResult SetTopProducts()
        {
            return View(db.ProductTypes.ToList());
        }
        public IActionResult TypeProductsItems(int TypeId)
        {
            return PartialView(db.Products.Where(x => x.ProductType_Id == TypeId).ToList());
        }
        //public IActionResult AddRemove TopProductCategory(int TypeId)
        //{

        //}
        //-----insert Brand----//
        public IActionResult InsertBrand()
        {
            return View();
        }
        public IActionResult InsertBrandConfirm(BrandViewModels models)
        {
            Brand objBrand = new Brand()
            {
                Name = models.Name
            };
            db.Add(objBrand);
            db.SaveChanges();
            return RedirectToAction("insert");
        }

    }

}

