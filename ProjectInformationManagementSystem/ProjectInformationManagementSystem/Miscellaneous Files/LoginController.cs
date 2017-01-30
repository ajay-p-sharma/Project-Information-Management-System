using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectInformationManagementSystem.Models;

namespace ProjectInformationManagementSystem.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Register()
        {
            return View();
        }



        [HttpPost]
        public ActionResult Register(user user)
        {
            if (ModelState.IsValid)
            {
                using (pimsEntitiesNew db = new pimsEntitiesNew())
                {
                    db.users.Add(user);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = user.first_name + " " + user.last_name + " successfully registered.";
            }
            return View();
        }



        public ActionResult Login()
        {
            return View();
        }


        
        public ActionResult Login(user user)
        {
            using (pimsEntitiesNew db = new pimsEntitiesNew())
            {
                var usr = db.users.Single(u=>u.email== user.email && u.password==user.password );

                if (usr != null)
                {
                    Session["email"] = user.email.ToString();
                    Session["userID"] = user.user_id.ToString();
                    return RedirectToAction("LoggedIn");
                }

                else
                {
                    ModelState.AddModelError("","Invalid username or password. Please try again.");
                }
            }
            return View();
        }

        public ActionResult LoggedIn()
        {
            if (Session["email"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
    }

}
