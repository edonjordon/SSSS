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
    public class RoomController : Controller
    {
        private SSSSContext db = new SSSSContext();

        //
        // GET: /Room/

        [Authorize]
        public ActionResult Index()
        {
            return View(db.Room.ToList());
        }

        //
        // GET: /Room/Details/5

        [Authorize]
        public ActionResult Details(int id = 0)
        {
            Room room = db.Room.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        //
        // GET: /Room/Create

        [Authorize]
        public ActionResult Create()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("_CreateRoom");
            }
            else
            {
                return View();
            }
        }

        //
        // POST: /Room/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Room room)
        {
            if (ModelState.IsValid)
            {
                room.RoomName = room.Building + " " + room.RoomNumber;
                db.Room.Add(room);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(room);
        }

        //
        // GET: /Room/Edit/5

        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            Room room = db.Room.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_EditRoom", room);
            }
            else
            {
                return View(room);
            }
        }

        //
        // POST: /Room/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Room room)
        {
            if (ModelState.IsValid)
            {
                room.RoomName = room.Building + " " + room.RoomNumber;
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(room);
        }

        //
        // GET: /Room/Delete/5

        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            Room room = db.Room.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_DeleteRoom", room);
            }
            else
            {
                return View(room);
            }
        }

        //
        // POST: /Room/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Room room = db.Room.Find(id);
            db.Room.Remove(room);
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