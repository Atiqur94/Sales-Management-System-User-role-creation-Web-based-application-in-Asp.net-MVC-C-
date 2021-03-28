using SalesReport.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AuthAuz.Models.ViewModels
{
    public class ProductCreateViewModel
    {
        public int ProductId { get; set; }
        [DisplayName("Product Name")]
        [Required(ErrorMessage = "Product name can't be empty")]
        [ExcludeCharacter("@?$")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Price can't be empty")]
        public string Price { get; set; }
        [DisplayName("Order Date")]
        [Required(ErrorMessage = "Order Date can't be empty")]
        [custom]
        public System.DateTime OrderDate { get; set; }
        [DisplayName("Image Name")]
        public string ImageName { get; set; }
        public string ImageUrl { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
    }
}