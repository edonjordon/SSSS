using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SS.Models
{
    public class Semester
    {
        [Key]
        public int SemesterId { get; set; }
        public string semesterName { get; set; } 
		public string Year { get; set; }
		public string Season { get; set; }

        //public virtual UserModel User { get; set; }

    }
}