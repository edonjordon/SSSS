using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SS.Models
{
    public class UserModel
    {
        [Key]
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Phone { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public int RoleID { get; set; }
        [ForeignKey("RoleID")]
        public virtual RoleModel Role { get; set; }
        //public virtual ICollection<RoleModel> Role { get; set; }
    }
}