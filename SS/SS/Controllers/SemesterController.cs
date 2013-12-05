using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SS.Models;
using SS.Helpers;

namespace SS.Controllers
{
    public class SemesterController : Controller
    {
        private SSSSContext db = new SSSSContext();

        [Authorize]
        public ActionResult ViewSchedule()
        {
            

            return View(db.Semester.ToList());
        }


        //
        // POST: /Semester/Copy

        [Authorize]
        public ActionResult Copy(int id)
        {


            CopySemesterHelper.copySemester(id, db);



            return RedirectToAction("Index");


        }
       

        //
        // GET: /Semester/

        [Authorize]
        public ActionResult Index()
        {
            return View(db.Semester.ToList());
        }

        //
        // GET: /Semester/Details/5

        [Authorize]
        public ActionResult Details(int id = 0)
        {
            Semester semester = db.Semester.Find(id);
            if (semester == null)
            {
                return HttpNotFound();
            }
            return View(semester);
        }

        //
        // GET: /Semester/Create

        [Authorize]
        public ActionResult Create()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("_CreateSemester");
            }
            else
            {
                return View();
            }
        }

        //
        // POST: /Semester/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Semester semester)
        {
            if (ModelState.IsValid)
            {
                semester.semesterName = semester.Season + " " + semester.Year;
                db.Semester.Add(semester);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(semester);
        }

        //
        // GET: /Semester/Edit/5

        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            Semester semester = db.Semester.Find(id);
            if (semester == null)
            {
                return HttpNotFound();
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_EditSemester",semester);
            }
            else
            {
            return View(semester);
            }
        }

        //
        // POST: /Semester/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Semester semester)
        {
            if (ModelState.IsValid)
            {
                semester.semesterName = semester.Season + " " + semester.Year;
                db.Entry(semester).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(semester);
        }

        //
        // GET: /Semester/Delete/5

        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            Semester semester = db.Semester.Find(id);
            if (semester == null)
            {
                return HttpNotFound();
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_DeleteSemester", semester);
            }
            else
            {
                return View(semester);
            }
        }

        //
        // POST: /Semester/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Semester semester = db.Semester.Find(id);
            db.Semester.Remove(semester);
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