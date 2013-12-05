using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SS.Models
{
    public class SectionConflict
    {
        [Key]
        public int SectionConflictId { get; set; }
        public bool roomConflict { get; set; }
        public bool cohortConflict { get; set; }
        public bool instructorConflict { get; set; }



        public int? SectionId { get; set; }
        [ForeignKey("SectionId")]
        public virtual Section parentSection { get; set; }


        public virtual Section sectionConflicted { get; set; }

              



    }
}