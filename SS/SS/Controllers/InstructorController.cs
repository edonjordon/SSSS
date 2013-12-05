using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SS.Models;

namespace SS.Controllers
{
    public class InstructorController : Controller
    {
        private SSSSContext db = new SSSSContext();

        //
        // GET: /Instructor/Print/5
        [EO.Pdf.Mvc4.RenderAsPDF]
        [Authorize]
        public ActionResult PrintSchedule(int semesterId = 0, int instructorId = 0)
        {
            Instructor instructor = db.Instructor.Find(instructorId);
            Semester semester = db.Semester.Find(semesterId);
            List<Section> sections = db.Section.Where(x => x.InstructorId == instructor.InstructorId && x.SemesterId == semester.SemesterId).ToList();
            if (instructor == null)
            {
                return HttpNotFound();
            }

            return View(sections);
        }
        
        
        
        //
        // GET: /Instructor/

        [Authorize]
        public ActionResult Index()
        {
            ViewBag.SemesterId = new SelectList(db.Semester, "SemesterId", "semesterName");
            return View(db.Instructor.ToList());
        }

        //
        // GET: /Instructor/Details/5

        [Authorize]
        public ActionResult Details(int id = 0)
        {
            Instructor instructor = db.Instructor.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        //
        // GET: /Instructor/Create

        [Authorize]
        public ActionResult Create()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("_CreateInstructor");
            }
            else
            {
                return View();
            }
        }

        //
        // POST: /Instructor/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                instructor.FullName = instructor.FirstName + " " + instructor.LastName;
                db.Instructor.Add(instructor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(instructor);
        }

        //
        // GET: /Instructor/Edit/5

        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            Instructor instructor = db.Instructor.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_EditInstructor", instructor);
            }
            else
            {
                return View(instructor);
            }
        }

        //
        // POST: /Instructor/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                instructor.FullName = instructor.FirstName + " " + instructor.LastName;
                db.Entry(instructor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(instructor);
        }

        //
        // GET: /Instructor/Delete/5

        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            Instructor instructor = db.Instructor.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_DeleteInstructor",instructor);
            }
            else
            {
                return View(instructor);
            }
        }

        //
        // POST: /Instructor/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Instructor instructor = db.Instructor.Find(id);
            db.Instructor.Remove(instructor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}