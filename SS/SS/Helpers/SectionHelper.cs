using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SS.Models;

namespace SS.Helpers
{
    public static class SectionHelper
    {
        private static SSSSContext db = new SSSSContext();

        public static List<Section> getSections(int anId)
        {

            Semester s = new Semester();
            s.SemesterId = anId;

            List<Section> semesterSections = db.Section.Where(x => x.SemesterId == s.SemesterId).ToList();

            return semesterSections;
        }

    }
}