using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SS.Models;
using SS.ViewModels;
using SS.Controllers;
using SS.Helpers;
using System.Data.Entity.Infrastructure;

namespace SS.Controllers
{
    public class SectionController : Controller
    {
        private SSSSContext db = new SSSSContext();

        //
        // GET: /Section/

        [Authorize]
        public ActionResult Scheduler(int id)
        {
            List<SectionConflictViewModel> vmList = new List<SectionConflictViewModel>();
            List<Section> sections = db.Section.Where(x => x.SemesterId == id).ToList();
            List<SectionConflict> conflicts = db.SectionConflict.ToList();
            foreach(Section s in sections){
                SectionConflictViewModel tempVM = new SectionConflictViewModel();
                tempVM.sectionConflicts = new List<SectionConflict>();
                tempVM.aSection = s;

                foreach( SectionConflict c in conflicts){
                    if(c.parentSection.SectionId == s.SectionId){
     
                            tempVM.sectionConflicts.Add(c);           
                    }
                }
                vmList.Add(tempVM);
            }
            return View(vmList);
        }

        //
        // GET: /Section/

        [Authorize]
        public ActionResult Index(List<ViewModels.SectionConflictViewModel> sectionVM)
        {
            List<SectionConflictViewModel> vmList = new List<SectionConflictViewModel>();
            List<Section> sections = db.Section.ToList();
            List<SectionConflict> conflicts = db.SectionConflict.ToList();
            foreach(Section s in sections){
                SectionConflictViewModel tempVM = new SectionConflictViewModel();
                tempVM.sectionConflicts = new List<SectionConflict>();
                tempVM.aSection = s;

                foreach( SectionConflict c in conflicts){
                    if(c.parentSection.SectionId == s.SectionId){
     
                            tempVM.sectionConflicts.Add(c);           
                    }
                }
                vmList.Add(tempVM);
            }
            
            //sectionVM.aSection = sections;
            //sectionVM.sectionConflicts = conflicts;         
            return View(vmList);
        }

        //
        // GET: /Section/Details/5

        [Authorize]
        public ActionResult Details(int id = 0)
        {
            Section section = db.Section.Find(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            return View(section);
        }

        //
        // GET: /Section/Create

        [Authorize]
        public ActionResult Create()
        {
            if (Request.IsAjaxRequest())
            {
                ViewBag.SemesterId = new SelectList(db.Semester, "SemesterId", "semesterName");
                ViewBag.RoomId = new SelectList(db.Room, "RoomId", "RoomName");
                ViewBag.CourseId = new SelectList(db.Course, "CourseId", "CourseName");
                ViewBag.InstructorId = new SelectList(db.Instructor, "InstructorId", "FullName");
                return PartialView("_CreateSection");
            }
            else
            {
                ViewBag.SemesterId = new SelectList(db.Semester, "SemesterId", "semesterName");
                ViewBag.RoomId = new SelectList(db.Room, "RoomId", "RoomName");
                ViewBag.CourseId = new SelectList(db.Course, "CourseId", "CourseName");
                ViewBag.InstructorId = new SelectList(db.Instructor, "InstructorId", "FullName");
                return View();
            }
        }

        //
        // POST: /Section/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Section section)
        {
            if (ModelState.IsValid)
            {
                SectionsOnInsert.initialize(section, section.SemesterId, db);
                return RedirectToAction("ViewSchedule", "Semester");
            }

            ViewBag.SemesterId = new SelectList(db.Semester, "SemesterId", "semesterName", section.SemesterId);
            ViewBag.RoomId = new SelectList(db.Room, "RoomId", "RoomName", section.RoomId);
            ViewBag.CourseId = new SelectList(db.Course, "CourseId", "CourseName", section.CourseId);
            ViewBag.InstructorId = new SelectList(db.Instructor, "InstructorId", "FullName", section.InstructorId);
            return View(section);
        }

        //
        // GET: /Section/Edit/5

        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            Section section = db.Section.Find(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            if (Request.IsAjaxRequest())
            {
                ViewBag.SemesterId = new SelectList(db.Semester, "SemesterId", "semesterName", section.SemesterId);
                ViewBag.RoomId = new SelectList(db.Room, "RoomId", "RoomName", section.RoomId);
                ViewBag.CourseId = new SelectList(db.Course, "CourseId", "CourseName", section.CourseId);
                ViewBag.InstructorId = new SelectList(db.Instructor, "InstructorId", "FullName", section.InstructorId);
                return PartialView("_EditSection",section);
            }
            else
            {
                ViewBag.SemesterId = new SelectList(db.Semester, "SemesterId", "semesterName", section.SemesterId);
                ViewBag.RoomId = new SelectList(db.Room, "RoomId", "RoomName", section.RoomId);
                ViewBag.CourseId = new SelectList(db.Course, "CourseId", "CourseName", section.CourseId);
                ViewBag.InstructorId = new SelectList(db.Instructor, "InstructorId", "FullName", section.InstructorId);
                return View(section);
            }
        }

        //
        // POST: /Section/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Section section)
        {
            if (ModelState.IsValid)
            {
                List<Section> semesterSections = db.Section.Where(x => x.SemesterId == section.SemesterId && x.SectionId != section.SectionId).ToList();
                //remove section from list
                //semesterSections.RemoveAll(i => i.SectionId == section.SectionId);
               
                
                
                //((IObjectContextAdapter)db).ObjectContext.Detach(semesterSections);
                RuleChecker.returnWrapper returnWrapperForSection = RuleChecker.initialize(section, section.SemesterId, semesterSections, db);
                //section = returnWrapperForSection.sectionChecked;

                foreach (Section aSection in returnWrapperForSection.listOfSections)
                {
                    if (aSection.SectionId != section.SectionId)
                    {
                        db.Entry(aSection).State = EntityState.Modified;
                    }
                    
                }

                foreach (SectionConflict aConflict in returnWrapperForSection.conflictList)
                {
                    List<SectionConflict> conflictCheck = db.SectionConflict.Where(x => x.parentSection.SectionId == aConflict.parentSection.SectionId && x.sectionConflicted.SectionId == aConflict.sectionConflicted.SectionId).ToList();
                        foreach( SectionConflict c in conflictCheck){
                            if (conflictCheck.Count() > 0 && conflictCheck.Count() <= 1)
                            {
                                c.roomConflict = aConflict.roomConflict;
                                c.instructorConflict = aConflict.instructorConflict;
                                c.cohortConflict = aConflict.cohortConflict;
                                if (c.cohortConflict == false && c.instructorConflict == false && c.roomConflict == false)
                                {
                                    section.HasConflicts = false;
                                }

                                db.Entry(c).State = EntityState.Modified;
                            }
                            if(conflictCheck.Count() == 0)
                            {
                                
                            }
                        }

                        db.SectionConflict.Add(aConflict);
                   
                }
                db.Entry(section).State = EntityState.Modified;
                //db.Section.Add(section);
                
                //DELETE OLD SHIT
                List<SectionConflict> conflicts = db.SectionConflict.Where(x => x.parentSection.SectionId == section.SectionId).ToList();
                List<SectionConflict> conflicts2 = db.SectionConflict.Where(x => x.sectionConflicted.SectionId == section.SectionId).ToList();

                foreach (SectionConflict c in conflicts)
                {
                    List<Section> conflictedSection = db.Section.Where(x => x.SectionId == c.parentSection.SectionId && x.SectionId != section.SectionId).ToList();
                    foreach (Section s in conflictedSection)
                    {
                        s.HasConflicts = false;
                        db.Entry(s).State = EntityState.Modified;
                    }
                    db.SectionConflict.Remove(c);
                }

                foreach (SectionConflict cf in conflicts2)
                {
                    List<Section> conflictedSection2 = db.Section.Where(x => x.SectionId == cf.parentSection.SectionId && x.SectionId != section.SectionId).ToList();

                    foreach (Section s in conflictedSection2)
                    {
                        List<SectionConflict> conflictCheckList = db.SectionConflict.Where(x => x.parentSection.SectionId == s.SectionId).ToList();
                        if (conflictCheckList.Count() == 0)
                        {
                            s.HasConflicts = false;
                            db.Entry(s).State = EntityState.Modified;
                        }
                    }
                    db.SectionConflict.Remove(cf);
                }
                db.SaveChanges();
                return RedirectToAction("ViewSchedule", "Semester");
            }

            ViewBag.SemesterId = new SelectList(db.Semester, "SemesterId", "semesterName", section.SemesterId);
            ViewBag.RoomId = new SelectList(db.Room, "RoomId", "RoomName", section.RoomId);
            ViewBag.CourseId = new SelectList(db.Course, "CourseId", "CourseName", section.CourseId);
            ViewBag.InstructorId = new SelectList(db.Instructor, "InstructorId", "FullName", section.InstructorId);
            return View(section);
        }

        //
        // GET: /Section/Delete/5

        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            Section section = db.Section.Find(id);
            if (Request.IsAjaxRequest())
            {

                return PartialView("_DeleteSection",section);
            }
            else
            {
                if (section == null)
                {
                    return HttpNotFound();
                }
                return View(section);
            }
        }

        //
        // POST: /Section/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SectionOnDelete.initialize(id, db);
            return RedirectToAction("ViewSchedule", "Semester");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}