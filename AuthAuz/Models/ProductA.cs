namespace AuthAuz.Models
{
    using SalesReport.Common;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("ProductA")]
    public partial class ProductA
    {
        [Key]
        public int ProductID { get; set; }
        [StringLength(50)]
        [Display(Name = "Product Name")]
        [Required(ErrorMessage ="Name must be entered")]
        [ExcludeCharacter("@?$")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Price must be entered")]
        [StringLength(50)]
        public string Price { get; set; }
        [Required(ErrorMessage = "Order Date must be entered")]
        [StringLength(50)]
        [Display(Name = "Order Date")]
        [custom]
        public string OrderDate { get; set; }

        [StringLength(500)]
        public string ImagePath { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageUpload { get; set; }

        public ProductA()
        {
            ImagePath = "~/AppFiles/Images/default.png";
        }
    }
}
