namespace AuthAuz.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblUsers
    {
        public tblUsers()
        {
            this.TblRoles = new HashSet<tblRoles>();
        }
        [Display(Name = "User Name")]
        public int ID { get; set; }
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }

        public string FullName { get; set; }
        public virtual ICollection<tblRoles> TblRoles { get; set; }
    }
}