using SS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SS.ViewModels
{
    public class ScheduleViewModel
    {
        public List<Semester> AvailableSemesters { get; set; }
        public List<Section> AvailableSections { get; set; }
    }
}