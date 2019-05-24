using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BehdadNematiFinalWebProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BehdadNematiFinalWebProject.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using BehdadNematiFinalWebProject.Areas.Identity.Data;
using System.Drawing;
using System.IO;

namespace BehdadNematiFinalWebProject.Controllers
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
                ImageThumbnailMaker imageThumbnailMaker = new ImageThumbnailMaker();
                p.ThumbnailImage = imageThumbnailMaker.CreateThumbNail(Image.FromStream(new MemoryStream(b)));
            }
            db.Add(p);
            if (db.SaveChanges() != 0)
            {
                foreach (var item in model.Pictures)
                {
                    if (item.Length <= 2 * Math.Pow(1024, 2))
                    {
                    }
                    else
                    {
                        return View("one or more Images may not be valid!");
                    }
                    var filename = System.IO.Path.GetExtension(item.FileName).ToLower();
                    if (filename == ".jpg" || filename == ".png")
                    {
                        byte[] b = new byte[item.Length];
                        item.OpenReadStream().Read(b, 0, b.Length);
                        image img = new image();
                        img.Img = b;
                        img.Product_Id = p.Id;
                        db.images.Add(img);
                    }
                    else
                    {
                        return View("one or more Images may not be valid!");

                    }
                };
                if (db.SaveChanges() != 0)
                {
                    return RedirectToAction("AddProduct");
                }
                else
                {
                    db.Products.Remove(p); //roll back
                    db.SaveChanges();
                }
            }
            return View("Error!");
        }

        //-------Edit Product-------//
        public IActionResult EditProduct(int id)
        {
            //Product p = db.Products.Find(id);
            Product p = db.Products.Include(x => x.images).Where(x => x.Id == id).First();
            if (p != null)
            {
                List<string> img = new List<string>();
                foreach (var item in p.images)
                {
                    img.Add($"data:image;base64,{Convert.ToBase64String(item.Img)}");
                }
                ProductViewModel ProductViewModel = new ProductViewModel()
                {
                    id = p.Id,
                    EnglishName = p.EnglishName,
                    Count = p.Count,
                    Price = p.Price,
                    Brand_Id = p.Brand_Id,
                    ProductType_Id = p.ProductType_Id,
                    ImagesBase64List = img
                };
                return View(ProductViewModel);

            }
            return View("Product not found!");
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
                ImageThumbnailMaker imageThumbnailMaker = new ImageThumbnailMaker();
                p.ThumbnailImage = imageThumbnailMaker.CreateThumbNail(Image.FromStream(new MemoryStream(b)));
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
            List<Product> p = db.Products.Include(x => x.images).Include(x => x.Brand).Include(x => x.ProductType).ToList();
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
        public IActionResult insertBrand()
        {
            return View();
        }
        public IActionResult insertBrandConfirm(BrandViewModels models)
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

