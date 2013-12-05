using SS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SS.Helpers
{
    public static class CopySemesterHelper
    {

        public static void copySemester(int id, SSSSContext db){
            
        Semester semester = db.Semester.Find(id);
        
        Semester copyOfSemester = new Semester();
        copyOfSemester.Season = semester.Season;
        copyOfSemester.Year = semester.Year;
        copyOfSemester.semesterName = copyOfSemester.Season + " "+ copyOfSemester.Year +  " --COPY";
        
        db.Semester.Add(copyOfSemester);


        List<Section> semesterSections = db.Section.Where(x => x.SemesterId == semester.SemesterId).ToList();
        List<Section> copiedSections = new List<Section>();


        foreach (Section s in semesterSections)
        {
            Section newSection = new Section();
            newSection.SemesterId = copyOfSemester.SemesterId;
            newSection.CourseId= s.CourseId;
            newSection.EndTime = s.EndTime;
            newSection.Friday = s.Friday;
            newSection.InstructorId = s.InstructorId;
            newSection.Monday = s.Monday;
            newSection.RoomId = s.RoomId;
            newSection.SectionCourse = s.SectionCourse;
            newSection.SectionNumber = s.SectionNumber;
            newSection.SemesterTaughtIn = copyOfSemester;
            newSection.StartTime = s.StartTime;
            newSection.TaughtBy = s.TaughtBy;
            newSection.TaughtInRoom = s.TaughtInRoom;
            newSection.Thursday = s.Thursday;
            newSection.Tuesday = s.Tuesday;
            newSection.Wednesday = s.Wednesday;

            db.Section.Add(newSection);
        }
        db.SaveChanges();
        }


    }
}