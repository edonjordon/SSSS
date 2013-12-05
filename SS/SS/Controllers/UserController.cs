using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SS.Models;
using System.Web.Helpers;

namespace SS.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class UserController : Controller
    {

        private SSSSContext db = new SSSSContext();

        //
        // GET: /User/

        public ActionResult Index()
        {
            //**ADD ROLES TO DATABASE**
            //RoleModel userrole = new RoleModel();
            //RoleModel adminrole = new RoleModel();
            //userrole.Name = "User";
            //adminrole.Name = "Admin";
            //db.Role.Add(userrole);
            //db.Role.Add(adminrole);
            //db.SaveChanges();

            var user = db.User.Include(u => u.Role);
            return View(user.ToList());
        }

        //
        // GET: /User/Details/5
        //[Authorize(Roles = "Admin")]
        public ActionResult Details(int id = 0)
        {
            UserModel usermodel = db.User.Find(id);
            if (usermodel == null)
            {
                return HttpNotFound();
            }
            return View(usermodel);
        }

        //
        // GET: /User/Edit/5
        //[Authorize(Roles="Admin")]
        public ActionResult ResetPassword(int id = 0)
        {
            UserModel usermodel = db.User.Find(id);
            if (usermodel == null)
            {
                return HttpNotFound();
            }
            if (Request.IsAjaxRequest())
            {
                ViewBag.useremail = usermodel.Email;
                ViewBag.userfname = usermodel.FirstName;
                ViewBag.userlname = usermodel.LastName;
                return PartialView("_ResetPassword", usermodel);
            }
            else
            {
                return View(usermodel);
            }
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        public ActionResult ResetPassword(UserModel usermodel)
        {
            if (ModelState.IsValid)
            {
                usermodel.Password = Crypto.HashPassword(usermodel.Password);
                db.Entry(usermodel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usermodel);
        }


        //
        // GET: /User/Create
        //[Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            if (Request.IsAjaxRequest())
            {
                ViewBag.RoleID = new SelectList(db.Role, "RoleID", "Name");
                return PartialView("_CreateUser");
            }
            else
            {
                ViewBag.RoleID = new SelectList(db.Role, "RoleID", "Name");
                return View();
            }
        }

        //
        // POST: /User/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserModel usermodel)
        {
            if (ModelState.IsValid)
            {
                usermodel.Password = Crypto.HashPassword(usermodel.Password);
                db.User.Add(usermodel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoleID = new SelectList(db.Role, "RoleID", "Name", usermodel.RoleID);
            return View(usermodel);
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id = 0)
        {
            UserModel usermodel = db.User.Find(id);
            if (usermodel == null)
            {
                return HttpNotFound();
            }
            if (Request.IsAjaxRequest())
            {
                //ViewBag.RoleID = new SelectList(db.Role, "RoleID", "Name");
                ViewBag.RoleID = new SelectList(db.Role, "RoleID", "Name", usermodel.RoleID);
                ViewBag.Role = usermodel.Role.Name;
                return PartialView("_EditUser", usermodel);
            }
            else
            {
                return View(usermodel);
            }
        }

        //
        // POST: /User/Edit/5
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserModel usermodel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usermodel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoleID = new SelectList(db.Role, "RoleID", "Name", usermodel.RoleID);
            return View(usermodel);
        }

        //
        // GET: /User/Delete/5
        //[Authorize(Roles = "Admin")]
        public ActionResult Delete(int id = 0)
        {
            UserModel usermodel = db.User.Find(id);
            if (usermodel == null)
            {
                return HttpNotFound();
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_DeleteUser", usermodel);
            }
            else
            {
                return View(usermodel);
            }
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserModel usermodel = db.User.Find(id);
            db.User.Remove(usermodel);
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