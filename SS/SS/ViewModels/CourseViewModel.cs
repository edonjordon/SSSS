using SS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SS.ViewModels
{
    public class CourseViewModel
    {
        //Course
        public string Department { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; }

        //Rules
        public string SeasonOffered { get; set; }
        public int? RoomCapacity { get; set; }
        public bool NeedsSpeakers { get; set; }
        public bool NeedsProjector { get; set; }
        public bool NeedsComputers { get; set; }

        public int? CourseId { get; set; }
        [ForeignKey("CourseId")]
        public virtual Course RuleFor { get; set; }

        public int? RoomId { get; set; }
        [ForeignKey("RoomId")]
        public virtual Room RoomTaughtIn { get; set; }

        public int? InstructorId { get; set; }
        [ForeignKey("InstructorId")]
        public virtual Instructor MustBeTaughtBy { get; set; }

    }
}