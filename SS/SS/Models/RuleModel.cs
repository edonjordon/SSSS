using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SS.Models
{
    public class CourseRule
    {
        [Key]
        public int RuleId { get; set; }
		public string SeasonOffered{ get; set; }
		public int ?RoomCapacity { get; set; }
		public bool NeedsSpeakers{ get; set; }
		public bool NeedsProjector{ get; set; }
		public bool NeedsComputers{ get; set; }
		
		public int ?CourseId { get; set; }
		[ForeignKey("CourseId")]
		public virtual Course RuleFor{ get; set; }
		
		public int ?RoomId{ get; set; }
		[ForeignKey("RoomId")]
		public virtual Room RoomTaughtIn{ get; set; }		
		
		public int ?InstructorId { get; set; }
		[ForeignKey("InstructorId")]
		public virtual Instructor MustBeTaughtBy{ get; set; }

    }
}