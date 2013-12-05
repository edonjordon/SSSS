using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SS.Models;
using SS.ViewModels;

namespace SS.Controllers
{
    public class CourseController : Controller
    {
        private SSSSContext db = new SSSSContext();

        //
        // GET: /Course/

        [Authorize]
        public ActionResult Index()
        {
            return View(db.Course.ToList());
        }

        //
        // GET: /Course/Details/5

        [Authorize]
        public ActionResult Details(int id = 0)
        {
            Course course = db.Course.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        //
        // GET: /Course/Create

        [Authorize]
        public ActionResult Create()
        {
            if (Request.IsAjaxRequest())
            {
                ViewBag.CourseId = new SelectList(db.Course, "CourseId", "CourseName");
                ViewBag.RoomId = new SelectList(db.Room, "RoomId", "RoomName");
                ViewBag.InstructorId = new SelectList(db.Instructor, "InstructorId", "FullName");
                return PartialView("_CreateCourse");
            }
            else
            {
                ViewBag.CourseId = new SelectList(db.Course, "CourseId", "CourseName");
                ViewBag.RoomId = new SelectList(db.Room, "RoomId", "RoomName");
                ViewBag.InstructorId = new SelectList(db.Instructor, "InstructorId", "FullName");
                return View();
            }
        }


        //
        // POST: /Course/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                course.CourseName = course.Subject+ " " + course.CourseNumber;
                db.Course.Add(course);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(course);
        }

        //
        // GET: /Course/Edit/5

        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            Course course = db.Course.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_EditCourse", course);
            }
            else
            {
                return View(course);
            }
        }

        //
        // POST: /Course/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Course course)
        {
            if (ModelState.IsValid)
            {
                course.CourseName = course.Subject + " " + course.CourseNumber;
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        //
        // GET: /Course/Delete/5

        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            Course course = db.Course.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_DeleteCourse",course);
            }
            else
            {
                return View(course);
            }
        }

        //
        // POST: /Course/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Course.Find(id);
            db.Course.Remove(course);
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