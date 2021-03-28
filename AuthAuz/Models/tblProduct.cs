namespace AuthAuz.Models
{
    using SalesReport.Common;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblProduct")]
    public partial class tblProduct
    {
        [Key]
        public int ProductId { get; set; }
        [ExcludeCharacter("@?$")]
        [StringLength(50)]
        [Required(ErrorMessage = "Product name can't be empty")]
        public string ProductName { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Price can't be empty")]
        public string Price { get; set; }
        [Required(ErrorMessage = "Order Date can't be empty")]
        [custom]
        public DateTime OrderDate { get; set; }

        public string ImageName { get; set; }

        public string ImageUrl { get; set; }
    }
}