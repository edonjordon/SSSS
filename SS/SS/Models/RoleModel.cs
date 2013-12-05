using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SS.Models
{
    public class RoleModel
    {
        [Key]
        public int RoleID { get; set; }
        public string Name { get; set; }

        //public virtual ICollection<UserModel> User { get; set; }
    }
}