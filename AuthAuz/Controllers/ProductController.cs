using AuthAuz.Models;
using AuthAuz.Models.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AuthAuz.Controllers
{
    public class ProductController : Controller
    {
        private AppDbContext db = new AppDbContext();
        [Authorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var emp = from s in db.tblProducts
                      select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                emp = emp.Where(s => s.ProductName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    emp = emp.OrderByDescending(s => s.ProductName);
                    break;

                default: 
                    emp = emp.OrderBy(s => s.ProductName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(emp.ToPagedList(pageNumber, pageSize));

            //List<ProductListViewModel> list = db.tblProducts.Select(t => new ProductListViewModel
            //{
            //    ProductId = t.ProductId,
            //    ProductName = t.ProductName,
            //    Price = t.Price,
            //    OrderDate = t.OrderDate,
            //    ImageName = t.ImageName,
            //    ImageUrl = t.ImageUrl
            //}).ToList();
            //return View(list);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult AddOrEdit(ProductCreateViewModel viewObj)
        {
            var result = false;
            string fileName = Path.GetFileNameWithoutExtension(viewObj.ImageFile.FileName);
            string extension = Path.GetExtension(viewObj.ImageFile.FileName);
            string fileWithExtension = fileName + extension;
            tblProduct trObj = new tblProduct();
            trObj.ProductName = viewObj.ProductName;
            trObj.Price = viewObj.Price;
            trObj.OrderDate = viewObj.OrderDate;
            trObj.ImageName = fileWithExtension;
            trObj.ImageUrl = "~/Images/" + fileName + extension;
            string serverPath = Path.Combine(Server.MapPath("~/Images/" + fileName + extension));
            viewObj.ImageFile.SaveAs(serverPath);
            if (ModelState.IsValid)
            {
                if (viewObj.ProductId == 0)
                {
                    db.tblProducts.Add(trObj);
                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    trObj.ProductId = viewObj.ProductId;
                    db.Entry(trObj).State = EntityState.Modified;
                    db.SaveChanges();
                    result = true;
                }
            }
            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                if (viewObj.ProductId == 0)
                {
                    return View("Create");
                }
                else
                {
                    return View("Edit");
                }
            }
        }
        public ActionResult Edit(int id)
        {
            tblProduct trObj = db.tblProducts.SingleOrDefault(t => t.ProductId == id);
            ProductCreateViewModel viewObj = new ProductCreateViewModel();
            viewObj.ProductId = trObj.ProductId;
            viewObj.ProductName = trObj.ProductName;
            viewObj.Price = trObj.Price;
            viewObj.OrderDate = trObj.OrderDate;
            viewObj.ImageUrl = trObj.ImageUrl;
            viewObj.ImageName = trObj.ImageName;
            return View(viewObj);
        }
        public ActionResult Delete(int id)
        {
            tblProduct trObj = db.tblProducts.SingleOrDefault(t => t.ProductId == id);
            {
                db.tblProducts.Remove(trObj);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}