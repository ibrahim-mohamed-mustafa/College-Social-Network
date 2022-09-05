using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using College_Social_Network_Project.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace College_Social_Network_Project.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentsWorldController : Controller
    {
        private Social_NetworkEntities db = new Social_NetworkEntities();

        // GET: StudentsWorld
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

        // GET: Students/Create
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            AspNetUser user = db.AspNetUsers.Find(userId);
            if (user.EmailConfirmed == true)
            {
                var useremail = User.Identity.GetUserName();
                TempData["userId"] = userId;
                TempData["useremail"] = useremail;
                var id = userId;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Student student = db.Students.Find(id);
                if (student == null)
                {
                    return RedirectToAction("Create1");

                    // return HttpNotFound();
                }

                ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", student.Sid);
                return View(student);          
            }
            else
            {
                return RedirectToAction("EmailConfirm", "Home");
            }

        }


        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Sid,Saddress,Semail,Sgender,Sacademicyear,course_id,con_id,RoleId")] Student student)
        {
            if (ModelState.IsValid)
            {
                //db.Students.Add(student);
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            ViewBag.course_id = new SelectList(db.Courses, "Cid", "Cdesc", student.course_id);
            ViewBag.RoleId = new SelectList(db.AspNetUserRoles, "RoleId", "RoleId", student.RoleId);
            return View(student);
        }

        // GET: Students/Create

        public ActionResult Create1()
        {

            var userId = User.Identity.GetUserId(); 
            AspNetUser user = db.AspNetUsers.Find(userId);
            if (user.EmailConfirmed == true)
            {
                var useremail = User.Identity.GetUserName();
                TempData["userId"] = userId;
                TempData["useremail"] = useremail;

                ViewBag.Sid = new SelectList(db.AspNetUserRoles, "UserId", "UserId");
                return View(); 
            }
            else
            {
                return RedirectToAction("EmailConfirm", "Home");
            }

        }

        // POST: Students/Create1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create1([Bind(Include = "Sid,Saddress,Semail,Sgender,Sacademicyear,course_id,con_id,RoleId")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.course_id = new SelectList(db.Courses, "Cid", "Cdesc", student.course_id);
            ViewBag.RoleId = new SelectList(db.AspNetUserRoles, "RoleId", "RoleId", student.RoleId);
            return View(student);
        }

        public ActionResult Course()
        {

            var id = User.Identity.GetUserId();
            AspNetUser user = db.AspNetUsers.Find(id);
            if (user.EmailConfirmed == true)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Student student = db.Students.Find(id);
                if (student == null)
                {
                    return RedirectToAction("Create1");

                }

                var list = from m in db.Join_course
                           where m.User_id == id
                           select m.Course;
                return View(list);
      
            }
            else
            {
                return RedirectToAction("EmailConfirm", "Home");
            }

        }
    }
}