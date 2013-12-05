using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SS.Models
{
    public class Instructor
    {
        [Key]
        public int InstructorId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
        public string FullName { get; set; }
		
		[StringLength(1, ErrorMessage = "Middle Initial cannot be longer than 1 character.")]
        public string MiddleName { get; set; }
		public string Department { get; set; }
		public bool Active{ get; set; }
    }
}