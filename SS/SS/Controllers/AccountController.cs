using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Helpers;
using SS.Models;

namespace SS.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        //public ActionResult Index()
        //{
        //    return View();
        //}
        private SSSSContext db = new SSSSContext();

        [HttpGet]
        [AllowAnonymous]
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(LogInModel user, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (IsValid(user.Email, user.Password))
                {
                    FormsAuthentication.SetAuthCookie(user.Email, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Email or Password is incorrect.");
                }
            }
            return View(user);
        }


        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home", null);
        }

        //Changing Password
        [HttpGet]
        //[ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
              : message == ManageMessageId.SetPasswordFailed ? " Something went wrong: Password has NOT been changed."
              : "";
            ViewBag.ReturnUrl = Url.Action("ChanagePassword");

            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel password)
        {

            bool changePasswordSucceeded = false;

            if (ModelState.IsValid)
            {
                string H_0_Password = password.OldPassword;
                string email = User.Identity.Name;

                if (IsValid(email, H_0_Password))
                {
                    db.User.Single(x => x.Email == email).Password = Crypto.HashPassword(password.ConfirmPassword);
                    db.SaveChanges();
                    changePasswordSucceeded = true;
                }/* end of if*/

            }
            else
            {
                ModelState.AddModelError(" ", "Password was NOT changed");
            }


            if (changePasswordSucceeded)
            {
                return RedirectToAction("ChangePassword", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            else
            {
                return RedirectToAction("ChangePassword", new { Message = ManageMessageId.SetPasswordFailed });
            }
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordFailed
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private bool IsValid(string email, string password)
        {
            bool isValid = false;

            //using (var db = new SSSSContext())
            {
                var user = db.User.FirstOrDefault(u => u.Email == email);

                if (user != null)
                {
                    if (Crypto.VerifyHashedPassword(user.Password, password))
                    {
                        isValid = true;
                    }
                }
            }
            return isValid;
        }
    }
}