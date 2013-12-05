using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SS.Models
{
    public class Section
    {
        [Key]
        public int SectionId { get; set; }
		public int SectionNumber{ get; set; }
		public string StartTime { get; set; }
		public string EndTime { get; set; }
		
		public bool Monday{ get; set; }
		public bool Tuesday{ get; set; }
		public bool Wednesday{ get; set; }
		public bool Thursday{ get; set; }
		public bool Friday{ get; set; }

        public bool HasConflicts { get; set; }

		public int SemesterId{ get; set; }
		[ForeignKey("SemesterId")]
		public virtual Semester SemesterTaughtIn{ get; set; }
		
		public int RoomId{ get; set; }
		[ForeignKey("RoomId")]
		public virtual Room TaughtInRoom{ get; set; }
		
		public int CourseId { get; set; }
		[ForeignKey("CourseId")]
		public virtual Course SectionCourse{ get; set; }
		
		public int InstructorId { get; set; }
		[ForeignKey("InstructorId")]
		public virtual Instructor TaughtBy{ get; set; }

    }
}