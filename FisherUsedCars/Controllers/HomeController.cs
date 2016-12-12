using FisherUsedCars.DAL;
using FisherUsedCars.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FisherUsedCars.Controllers
{
    public class HomeController : Controller
    {

        FisherUsedCarsContext db = new FisherUsedCarsContext(); //DO NOT FORGET THIS

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection form, bool rememberMe = false)
        {
            String email = form["Email address"].ToString();
            String password = form["Password"].ToString();

            bool validUser = db.User.Any(m => m.email.Equals(email));

            if(validUser)
            {
                Users tempUser = db.User.SingleOrDefault(m => m.email.Equals(email));

                int myUserID = tempUser.userID;
                Session["TESLA"] = myUserID;

                if(string.Equals(tempUser.password, password))
                {
                    FormsAuthentication.SetAuthCookie(email, rememberMe);
                    return RedirectToAction("Index", "Home"); //User is authenticated succesfully
                }
                else
                {
                    ViewBag.error = "The password entered is incorrect.";
                    ViewBag.alert = true;
                    return View();
                }
            }
            else
            {
                ViewBag.error = "The email entered does not match any on file.";
                ViewBag.alert = true;
                return View();
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "userID, email, password, firstName, lastName")] Users User, FormCollection Form, bool rememberMe = false)
        {
            if(ModelState.IsValid)
            {
                String email = Form["email"].ToString();
                db.User.Add(User);
                db.SaveChanges();

                FormsAuthentication.SetAuthCookie(email, rememberMe);

                int myUserID = User.userID;
                Session["TESLA"] = myUserID;

                return RedirectToAction("Index", "Home");
            }
            return View(User);
        }
    }
}