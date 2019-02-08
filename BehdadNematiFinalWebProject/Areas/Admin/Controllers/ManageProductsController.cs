using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BehdadNematiFinalWebProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BehdadNematiFinalWebProject.Models.viewModels;

namespace BehdadNematiFinalWebProject.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "admins")]
    public class ManageProductsController : Controller
    {
        ApplicationContext db;
        public ManageProductsController(ApplicationContext _db)
        {
            db = _db;
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
            return PartialView(db.brands.ToList());
        }
        //---------Add Product---------//
        public IActionResult AddProduct()
        {
            return View();
        }

        public IActionResult AddProductConfirm(ProductViewModel obj)
        {
            product p = new product()
            {
                EnglishName = obj.EnglishName,
                price = obj.price,
                count=obj.count,
                productType_Id=obj.productType_Id,
                brand_Id=obj.brand_Id        
            };
            db.Add(p);
            if (db.SaveChanges()!=0)
            {
                foreach (var item in obj.Pictures)
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
                            img.img = b;
                            img.product_Id = p.Id;
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
                    db.products.Remove(p); //roll back
                    db.SaveChanges();
                }
            }
            return View("Error!");
        }

        //-------Edit Product-------//
        public IActionResult EditProduct(int id)
        {
            product p = db.products.Find(id);
            if (p!=null)
            {
                ProductViewModel productView = new ProductViewModel()
                {
                    id = p.Id,
                    EnglishName=p.EnglishName,
                    count=p.count,
                    price=p.price,
                    brand_Id=p.brand_Id,
                    productType_Id=p.productType_Id
                };
                return View(productView);

            }
            return View("Product not found!");
        }
        public IActionResult EditProductConfirm(ProductViewModel obj)
        {
            product p =  db.products.Find(obj.id);
            p.EnglishName = obj.EnglishName;
            p.brand_Id = obj.brand_Id;
            p.productType_Id = obj.productType_Id;
            p.count = obj.count;
            p.price = obj.price;
            if (db.SaveChanges()!=0)
            {
                return RedirectToAction("ProductList");
            }
            return View("Error!");
        }
        //-------Delete Product-----//
        public IActionResult DeleteProduct(int id)
        {
            product p = db.products.Find(id);
            if (p!=null)
            {
                db.Remove(p);
                if (db.SaveChanges()!=0)
                {
                    return RedirectToAction("productlist");
                    return RedirectToAction("productlist");
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
            List<product> p = new List<product>();
            if (Search==null)
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
        public List<product> FindProductByBrandType(int TypeId, int BrandId)
        {
            List<product> p = db.products.Include(x => x.images).Include(x => x.brand).Include(x => x.productType).ToList();
            if (TypeId == 0 && BrandId == 0) //brand and type are not specified
            {
                return p;
            }
            else if (TypeId != 0 && BrandId == 0) //brand is not specified
            {
                return p.Where(x => x.productType_Id == TypeId).ToList();
            }
            else if (BrandId != 0 && TypeId == 0) //Type is not specified
            {
                return p.Where(x => x.brand_Id == BrandId).ToList();
            }
            else //both brand and type are selected
            {
                return p.Where(x => x.brand_Id == BrandId && x.productType_Id == TypeId).ToList();
            }
            
        }
        //------select Top products----//
        public IActionResult SetTopProducts()
        {    
            return View(db.ProductTypes.ToList()); 
        }
        public IActionResult TypeProductsItems(int TypeId)
        {
            return PartialView(db.products.Where(x=>x.productType_Id==TypeId).ToList());
        }
        //public IActionResult AddRemove TopProductCategory(int TypeId)
        //{

        //}
    }
}