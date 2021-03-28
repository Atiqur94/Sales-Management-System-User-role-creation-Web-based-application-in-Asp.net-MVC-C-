using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthAuz.Models.ViewModels
{
    public class VmCategoryWiseProduct
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<VmCategory> CategoryList { get; set; }
        public List<VmProduct> ProductList { get; set; }
    }
}