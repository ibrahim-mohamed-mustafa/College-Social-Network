using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace College_Social_Network_Project.Controllers
{
    public class HomeController : Controller
    {
       
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            TempData["userId"] = userId;


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

        public ActionResult EmailConfirm()
        {
            TempData["Error"]= "Please confirm your account by link sent to your Email";
            TempData.Keep();
            return View();
        }






    }
}