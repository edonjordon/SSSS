using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SS.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseNumber { get; set; }
		public string Department { get; set; }
		public string Description { get; set; }
		public string Subject { get; set; }
        
    }
}