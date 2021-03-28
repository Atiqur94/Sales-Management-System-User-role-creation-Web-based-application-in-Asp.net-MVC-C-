namespace AuthAuz.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblRoles
    {
        public int ID { get; set; }

        public string RoleName { get; set; }

        public int UserID { get; set; }

        public virtual tblUsers TblUser { get; set; }
    }
}