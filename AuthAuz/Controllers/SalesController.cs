using AuthAuz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AuthAuz.Controllers
{
    public class SalesController : Controller
    {
        [Authorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Index()
        {
            var db = new AppDbContext();
            return View(db.Customers.ToList());
        }
        public ActionResult Create()
        {
            var model = new Customer();
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(Customer model, string[] ProductName, int[] Quantity, decimal[] Price)
        {
            var db = new AppDbContext();

            db.Customers.Add(model);
            db.SaveChanges();

            var listProduct = new List<Product>();
            for (int i = 0; i < ProductName.Length; i++)
            {
                var newProduct = new Product();
                newProduct.CustomerID = model.CustomerID;
                newProduct.ProductName = ProductName[i];
                newProduct.Quantity = Quantity[i];
                newProduct.Price = Price[i];
                listProduct.Add(newProduct);
            }
            db.Products.AddRange(listProduct);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            var db = new AppDbContext();
            var oCustomer = db.Customers.Where(c => c.CustomerID == id).FirstOrDefault();
            ViewBag.ProductList = db.Products.Where(c => c.CustomerID == id).ToList();
            return View(oCustomer);
        }
        [HttpPost]
        public ActionResult Edit(Customer model, string[] ProductName, int[] Quantity, decimal[] Price)
        {
            var db = new AppDbContext();

            var oCustomer = db.Customers.Where(c => c.CustomerID == model.CustomerID).FirstOrDefault();
            oCustomer.Address = model.Address;
            oCustomer.CustomerName = model.CustomerName;
            oCustomer.Mobile = model.Mobile;
            oCustomer.SaleDate = model.SaleDate;
            
            var listProductDel = db.Products.Where(p => p.CustomerID == model.CustomerID).ToList();
            db.Products.RemoveRange(listProductDel);
            db.SaveChanges();

            
            var listProduct = new List<Product>();
            for (int i = 0; i < ProductName.Length; i++)
            {
                if (Quantity[i] > 0)
                {
                    var newProduct = new Product();
                    newProduct.CustomerID = model.CustomerID;
                    newProduct.ProductName = ProductName[i];
                    newProduct.Quantity = Quantity[i];
                    newProduct.Price = Price[i];
                    listProduct.Add(newProduct);
                }
            }
            db.Products.AddRange(listProduct);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            var db = new AppDbContext();

            var listProductDel = db.Products.Where(p => p.CustomerID == id).ToList();
            db.Products.RemoveRange(listProductDel);

            var oCustomer = db.Customers.Where(c => c.CustomerID == id).FirstOrDefault();
            db.Customers.Remove(oCustomer);

            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}