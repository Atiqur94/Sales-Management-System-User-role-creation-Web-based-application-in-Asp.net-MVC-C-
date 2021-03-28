using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AuthAuz.Models
{
    public class MProduct
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
        public DateTime? OrderDate { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
    }
}