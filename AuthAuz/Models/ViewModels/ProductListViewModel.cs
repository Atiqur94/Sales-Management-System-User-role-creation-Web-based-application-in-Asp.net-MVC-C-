using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthAuz.Models.ViewModels
{
    public class ProductListViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Price { get; set; }
        public System.DateTime OrderDate { get; set; }
        public string ImageName { get; set; }
        public string ImageUrl { get; set; }
    }
}