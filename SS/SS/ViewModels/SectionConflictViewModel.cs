using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SS.Models;

namespace SS.ViewModels
{
    public class SectionConflictViewModel
    {

        public Section aSection { get; set; }
        public List<SectionConflict> sectionConflicts{ get; set; }

    }
}