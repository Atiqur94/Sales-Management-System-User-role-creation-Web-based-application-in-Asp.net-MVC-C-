using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthAuz.Models.ViewModels
{
    public class VmProduct
    {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public decimal Price { get; set; }
            public string ImagePath { get; set; }
            public DateTime? OrderDate { get; set; }
            public int CategoryId { get; set; }
            public int Quantity { get; set; }
            public string CategoryName { get; set; }
            public HttpPostedFile ImgFile { get; set; }
            public List<Category> CategoryList { get; set; }
        }
}
