using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SS.Models;

using System.IO;
using iTextSharp;
//using iTextSharp.text;
using EO.Pdf.Mvc4;

namespace SS.Controllers
{
    public class PdfViewController : Controller
    {
        private SSSSContext db = new SSSSContext();

      






        //
        // GET: /PdfView/
        [EO.Pdf.Mvc4.RenderAsPDFAttribute]
        public ActionResult Index()
        {
            var section = db.Section.Include(s => s.SemesterTaughtIn).Include(s => s.TaughtInRoom).Include(s => s.SectionCourse).Include(s => s.TaughtBy);
            return View(section.ToList());
        }

        //
        // GET: /PdfView/Details/5

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
        // GET: /PdfView/Create

        public ActionResult Create()
        {
            ViewBag.SemesterId = new SelectList(db.Semester, "SemesterId", "semesterName");
            ViewBag.RoomId = new SelectList(db.Room, "RoomId", "RoomName");
            ViewBag.CourseId = new SelectList(db.Course, "CourseId", "CourseName");
            ViewBag.InstructorId = new SelectList(db.Instructor, "InstructorId", "FirstName");
            return View();
        }

        //
        // POST: /PdfView/Create

        [HttpPost]
        public ActionResult Create(Section section)
        {
            if (ModelState.IsValid)
            {
                db.Section.Add(section);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SemesterId = new SelectList(db.Semester, "SemesterId", "semesterName", section.SemesterId);
            ViewBag.RoomId = new SelectList(db.Room, "RoomId", "RoomName", section.RoomId);
            ViewBag.CourseId = new SelectList(db.Course, "CourseId", "CourseName", section.CourseId);
            ViewBag.InstructorId = new SelectList(db.Instructor, "InstructorId", "FirstName", section.InstructorId);
            return View(section);
        }

        //
        // GET: /PdfView/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Section section = db.Section.Find(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            ViewBag.SemesterId = new SelectList(db.Semester, "SemesterId", "semesterName", section.SemesterId);
            ViewBag.RoomId = new SelectList(db.Room, "RoomId", "RoomName", section.RoomId);
            ViewBag.CourseId = new SelectList(db.Course, "CourseId", "CourseName", section.CourseId);
            ViewBag.InstructorId = new SelectList(db.Instructor, "InstructorId", "FirstName", section.InstructorId);
            return View(section);
        }

        //
        // POST: /PdfView/Edit/5

        [HttpPost]
        public ActionResult Edit(Section section)
        {
            if (ModelState.IsValid)
            {
                db.Entry(section).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SemesterId = new SelectList(db.Semester, "SemesterId", "semesterName", section.SemesterId);
            ViewBag.RoomId = new SelectList(db.Room, "RoomId", "RoomName", section.RoomId);
            ViewBag.CourseId = new SelectList(db.Course, "CourseId", "CourseName", section.CourseId);
            ViewBag.InstructorId = new SelectList(db.Instructor, "InstructorId", "FirstName", section.InstructorId);
            return View(section);
        }

        //
        // GET: /PdfView/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Section section = db.Section.Find(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            return View(section);
        }

        //
        // POST: /PdfView/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Section section = db.Section.Find(id);
            db.Section.Remove(section);
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