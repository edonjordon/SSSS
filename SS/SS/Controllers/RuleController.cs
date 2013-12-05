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
    public class RuleController : Controller
    {
        private SSSSContext db = new SSSSContext();

        //
        // GET: /Rule/

        [Authorize]
        public ActionResult Index()
        {
            var rule = db.Rule.Include(c => c.RuleFor).Include(c => c.RoomTaughtIn).Include(c => c.MustBeTaughtBy);
            return View(rule.ToList());
        }

        //
        // GET: /Rule/Details/5

        [Authorize]
        public ActionResult Details(int id = 0)
        {
            CourseRule courserule = db.Rule.Find(id);
            if (courserule == null)
            {
                return HttpNotFound();
            }
            return View(courserule);
        }

        //
        // GET: /Rule/Create

        [Authorize]
        public ActionResult Create()
        {
            if (Request.IsAjaxRequest())
            {
                ViewBag.CourseId = new SelectList(db.Course, "CourseId", "CourseName");
                ViewBag.RoomId = new SelectList(db.Room, "RoomId", "RoomName");
                ViewBag.InstructorId = new SelectList(db.Instructor, "InstructorId", "FullName");
                return PartialView("_CreateRule");
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
        // POST: /Rule/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseRule courserule)
        {
            if (ModelState.IsValid)
            {
                db.Rule.Add(courserule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(db.Course, "CourseId", "CourseName", courserule.CourseId);
            ViewBag.RoomId = new SelectList(db.Room, "RoomId", "RoomName", courserule.RoomId);
            ViewBag.InstructorId = new SelectList(db.Instructor, "InstructorId", "FullName", courserule.InstructorId);
            return View(courserule);
        }

        //
        // GET: /Rule/Edit/5

        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            CourseRule courserule = db.Rule.Find(id);
            if (courserule == null)
            {
                return HttpNotFound();
            }
            if (Request.IsAjaxRequest())
            {
                ViewBag.CourseId = new SelectList(db.Course, "CourseId", "CourseName", courserule.CourseId);
                ViewBag.RoomId = new SelectList(db.Room, "RoomId", "RoomName", courserule.RoomId);
                ViewBag.InstructorId = new SelectList(db.Instructor, "InstructorId", "FullName", courserule.InstructorId);
                return PartialView("_EditRule", courserule);
            }
            else
            {
                ViewBag.CourseId = new SelectList(db.Course, "CourseId", "CourseName", courserule.CourseId);
                ViewBag.RoomId = new SelectList(db.Room, "RoomId", "RoomName", courserule.RoomId);
                ViewBag.InstructorId = new SelectList(db.Instructor, "InstructorId", "FullName", courserule.InstructorId);
                return View(courserule);
            }
        }

        //
        // POST: /Rule/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CourseRule courserule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(courserule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Course, "CourseId", "CourseName", courserule.CourseId);
            ViewBag.RoomId = new SelectList(db.Room, "RoomId", "RoomName", courserule.RoomId);
            ViewBag.InstructorId = new SelectList(db.Instructor, "InstructorId", "FullName", courserule.InstructorId);
            return View(courserule);
        }

        //
        // GET: /Rule/Delete/5

        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            CourseRule courserule = db.Rule.Find(id);
            if (courserule == null)
            {
                return HttpNotFound();
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_DeleteRule");
            }
            else
            {
                return View(courserule);
            }
        }

        //
        // POST: /Rule/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CourseRule courserule = db.Rule.Find(id);
            db.Rule.Remove(courserule);
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