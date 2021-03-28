using AuthAuz.Models;
using AuthAuz.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AuthAuz.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Index(int? id)
        {
            var ctx = new AppDbContext();

            var categoryWiseProductQty = from p in ctx.MProducts
                                         group p by p.CategoryId into g
                                         select new
                                         {
                                             g.FirstOrDefault().CategoryId,
                                             Qty = g.Sum(s => s.Quantity)
                                         };
            var listCategory = (from c in ctx.Categories
                                join cwpq in categoryWiseProductQty on c.CategoryId equals cwpq.CategoryId
                                select new VmCategory
                                {
                                    CategoryName = c.CategoryName,
                                    CategoryId = cwpq.CategoryId,
                                    Quantity = cwpq.Qty
                                }).ToList();
            var listProduct = (from p in ctx.MProducts
                               join c in ctx.Categories on p.CategoryId equals c.CategoryId
                               where p.CategoryId == id
                               select new VmProduct
                               {
                                   CategoryId = p.CategoryId,
                                   CategoryName = c.CategoryName,
                                   OrderDate = p.OrderDate,
                                   ImagePath = p.ImagePath,
                                   Price = p.Price,
                                   ProductId = p.ProductId,
                                   ProductName = p.ProductName,
                                   Quantity = p.Quantity
                               }).ToList();

            var oCategoryWiseProduct = new VmCategoryWiseProduct();
            oCategoryWiseProduct.CategoryList = listCategory;
            oCategoryWiseProduct.ProductList = listProduct;
            oCategoryWiseProduct.CategoryId = listProduct.Count > 0 ? listProduct[0].CategoryId : 0;
            oCategoryWiseProduct.CategoryName = listProduct.Count > 0 ? listProduct[0].CategoryName : "";

            return View(oCategoryWiseProduct);
        }
        public ActionResult Create()
        {
            var model = new VmProductCategory();
            var ctx = new AppDbContext();
            model.CategoryList = ctx.Categories.ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(Category model, string[] ProductName, decimal[] Price, int[] Quantity, DateTime[] OrderDate, HttpPostedFileBase[] imgFile)
        {
            var ctx = new AppDbContext();
            var oCatetory = (from c in ctx.Categories where c.CategoryName == model.CategoryName.Trim() select c).FirstOrDefault();
            if (oCatetory == null)
            {
                ctx.Categories.Add(model);
                ctx.SaveChanges();
            }
            else
            {
                model.CategoryId = oCatetory.CategoryId;
            }

            var listProduct = new List<MProduct>();
            for (int i = 0; i < ProductName.Length; i++)
            {
                string imgPath = "";
                if (imgFile[i] != null && imgFile[i].ContentLength > 0)
                {
                    var fileName = Path.GetFileName(imgFile[i].FileName);
                    string fileLocation = Path.Combine(
                        Server.MapPath("~/uploads"), fileName);
                    imgFile[i].SaveAs(fileLocation);

                    imgPath = "/uploads/" + imgFile[i].FileName;
                }

                var newProduct = new MProduct();
                newProduct.ProductName = ProductName[i];
                newProduct.Quantity = Quantity[i];
                newProduct.Price = Price[i];
                newProduct.OrderDate = OrderDate[i];
                newProduct.ImagePath = imgPath;
                newProduct.Quantity = Quantity[i];
                newProduct.CategoryId = model.CategoryId;
                listProduct.Add(newProduct);
            }
            ctx.MProducts.AddRange(listProduct);
            ctx.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            var ctx = new AppDbContext();
            var oProduct = (from p in ctx.MProducts
                            join c in ctx.Categories on p.CategoryId equals c.CategoryId
                            where p.ProductId == id
                            select new VmProduct
                            {
                                CategoryId = p.CategoryId,
                                CategoryName = c.CategoryName,
                                OrderDate = p.OrderDate,
                                ImagePath = p.ImagePath,
                                Price = p.Price,
                                ProductId = p.ProductId,
                                ProductName = p.ProductName,
                                Quantity = p.Quantity
                            }).FirstOrDefault();
            oProduct.CategoryList = ctx.Categories.ToList(); 
            return View(oProduct);
        }
        [HttpPost]
        public ActionResult Edit(VmProduct model)
        {
            var ctx = new AppDbContext();
            var oCatetory = (from c in ctx.Categories where c.CategoryName == model.CategoryName.Trim() select c).FirstOrDefault();
            if (oCatetory == null)
            {
                oCatetory = new Category();
                oCatetory.CategoryName = model.CategoryName;
                ctx.Categories.Add(oCatetory);
                ctx.SaveChanges();
            }

            string imgPath = "";
            if (model.ImgFile != null && model.ImgFile.ContentLength > 0)
            {
                var fileName = Path.GetFileName(model.ImgFile.FileName);
                string fileLocation = Path.Combine(
                    Server.MapPath("~/uploads"), fileName);
                model.ImgFile.SaveAs(fileLocation);

                imgPath = "/uploads/" + model.ImgFile.FileName;
            }

            var oProduct = ctx.MProducts.Where(w => w.ProductId == model.ProductId).FirstOrDefault();
            if (oProduct != null)
            {
                oProduct.ProductName = model.ProductName;
                oProduct.Quantity = model.Quantity;
                oProduct.Price = model.Price;
                oProduct.OrderDate = model.OrderDate;
                oProduct.CategoryId = oCatetory.CategoryId;
                if (!string.IsNullOrEmpty(imgPath))
                {
                    var fileName = Path.GetFileName(oProduct.ImagePath);
                    string fileLocation = Path.Combine(Server.MapPath("~/uploads"), fileName);
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }
                }
                oProduct.ImagePath = imgPath == "" ? oProduct.ImagePath : imgPath;

                ctx.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult EditMultiple(int id)
        {
            var ctx = new AppDbContext();
            var oCategoryWiseProduct = new VmCategoryWiseProduct();
            var listProduct = (from p in ctx.MProducts
                               join c in ctx.Categories on p.CategoryId equals c.CategoryId
                               where p.CategoryId == id
                               select new VmProduct
                               {
                                   CategoryId = p.CategoryId,
                                   CategoryName = c.CategoryName,
                                   OrderDate = p.OrderDate,
                                   ImagePath = p.ImagePath,
                                   Price = p.Price,
                                   ProductId = p.ProductId,
                                   ProductName = p.ProductName,
                                   Quantity = p.Quantity
                               }).ToList();
            oCategoryWiseProduct.ProductList = listProduct;
            
            oCategoryWiseProduct.CategoryList = (from c in ctx.Categories
                                                 select new VmCategory
                                                 {
                                                     CategoryId = c.CategoryId,
                                                     CategoryName = c.CategoryName
                                                 }).ToList();
            oCategoryWiseProduct.CategoryId = listProduct.Count > 0 ? listProduct[0].CategoryId : 0;
            oCategoryWiseProduct.CategoryName = listProduct.Count > 0 ? listProduct[0].CategoryName : "";
            return View(oCategoryWiseProduct);
        }

        [HttpPost]
        public ActionResult EditMultiple(Category model, int[] ProductId, string[] ProductName, decimal[] Price, int[] Quantity, DateTime[] OrderDate, HttpPostedFileBase[] imgFile)
        {
            var ctx = new AppDbContext();
            var listProduct = new List<Product>();
            for (int i = 0; i < ProductName.Length; i++)
            {
                if (ProductId[i] > 0)
                {
                    string imgPath = "";
                    if (imgFile[i] != null && imgFile[i].ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(imgFile[i].FileName);
                        string fileLocation = Path.Combine(
                            Server.MapPath("~/uploads"), fileName);
                        imgFile[i].SaveAs(fileLocation);

                        imgPath = "/uploads/" + imgFile[i].FileName;
                    }
                    int pid = ProductId[i];
                    var oProduct = ctx.MProducts.Where(w => w.ProductId == pid).FirstOrDefault();
                    if (oProduct != null)
                    {
                        oProduct.ProductName = ProductName[i];
                        oProduct.Quantity = Quantity[i];
                        oProduct.Price = Price[i];
                        oProduct.OrderDate = OrderDate[i];
                        oProduct.CategoryId = model.CategoryId;
                        if (!string.IsNullOrEmpty(imgPath))
                        {
                            var fileName = Path.GetFileName(oProduct.ImagePath);
                            string fileLocation = Path.Combine(Server.MapPath("~/uploads"), fileName);
                            if (System.IO.File.Exists(fileLocation))
                            {
                                System.IO.File.Delete(fileLocation);
                            }
                        }
                        oProduct.ImagePath = imgPath == "" ? oProduct.ImagePath : imgPath;
                        ctx.SaveChanges();
                    }
                }
                else if (!string.IsNullOrEmpty(ProductName[i]))
                {
                    string imgPath = "";
                    if (imgFile[i] != null && imgFile[i].ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(imgFile[i].FileName);
                        string fileLocation = Path.Combine(
                            Server.MapPath("~/uploads"), fileName);
                        imgFile[i].SaveAs(fileLocation);

                        imgPath = "/uploads/" + imgFile[i].FileName;
                    }

                    var newProduct = new MProduct();
                    newProduct.ProductName = ProductName[i];
                    newProduct.Quantity = Quantity[i];
                    newProduct.Price = Price[i];
                    newProduct.OrderDate = OrderDate[i];
                    newProduct.ImagePath = imgPath;
                    newProduct.Quantity = Quantity[i];
                    newProduct.CategoryId = model.CategoryId;
                    ctx.MProducts.Add(newProduct);
                    ctx.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var ctx = new AppDbContext();
            var oProduct = ctx.MProducts.Where(p => p.ProductId == id).FirstOrDefault();
            if (oProduct != null)
            {
                ctx.MProducts.Remove(oProduct);
                ctx.SaveChanges();

                var fileName = Path.GetFileName(oProduct.ImagePath);
                string fileLocation = Path.Combine(
                    Server.MapPath("~/uploads"), fileName);
                    
                if (System.IO.File.Exists(fileLocation))
                {
                        
                    System.IO.File.Delete(fileLocation);
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult DeleteMultiple(int id)
        {
            var ctx = new AppDbContext();
            var listProduct = ctx.MProducts.Where(p => p.CategoryId == id).ToList();
            foreach (var oProduct in listProduct)
            {
                if (oProduct != null)
                {
                    ctx.MProducts.Remove(oProduct);
                    ctx.SaveChanges();

                    var fileName = Path.GetFileName(oProduct.ImagePath);
                    string fileLocation = Path.Combine(
                        Server.MapPath("~/uploads"), fileName);
                        
                    if (System.IO.File.Exists(fileLocation))
                    {
                           
                        System.IO.File.Delete(fileLocation);
                    }
                }
            }

            var oCategory = ctx.Categories.Where(c => c.CategoryId == id).FirstOrDefault();
            ctx.Categories.Remove(oCategory);
            ctx.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}