using College_Social_Network_Project.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace College_Social_Network_Project.Controllers
{
    [Authorize(Roles = "Admin")]
    
    public class DashboardController : Controller
    {
        // GET: Dashboard
        private Social_NetworkEntities db = new Social_NetworkEntities();

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            AspNetUser user = db.AspNetUsers.Find(userId);
            if (user.EmailConfirmed == true)
            {
                return View();
            }
            else
            {
                return RedirectToAction("EmailConfirm", "Home");

            }
        }

        public ActionResult test()
        {
            return View();
        }

    }
}