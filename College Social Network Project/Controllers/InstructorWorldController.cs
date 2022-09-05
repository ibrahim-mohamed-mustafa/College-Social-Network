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
    [Authorize(Roles = "Instructor")]
    public class InstructorWorldController : Controller
    {
        private Social_NetworkEntities db = new Social_NetworkEntities();

        // GET: InstructorWorld
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            AspNetUser user = db.AspNetUsers.Find(userId);
            if (user.EmailConfirmed == true)
            {
                return View();
            }
            else {
                return RedirectToAction("EmailConfirm", "Home");
               
            }
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            var useremail = User.Identity.GetUserName();
            TempData["userId"] = userId;
            TempData["useremail"] = useremail;
            var id = userId;
           
            AspNetUser user = db.AspNetUsers.Find(userId);
            if (user.EmailConfirmed == true)
            {
              
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Instructor Instructor = db.Instructors.Find(id);
                if (Instructor == null)
                {
                    return RedirectToAction("Create1");

                // return HttpNotFound();
                }

                ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", Instructor.Iid);
                return View(Instructor);
            }
             else
            {
                return RedirectToAction("EmailConfirm", "Home");
            }


        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Iid,Iaddress,Iemail,Igender,Iofficehours,course_id,con_id,RoleId")] Instructor Instructor)
        {
            if (ModelState.IsValid)
            {
                //db.Students.Add(student);
                db.Entry(Instructor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }



            ViewBag.course_id = new SelectList(db.Courses, "Cid", "Cdesc");
            ViewBag.RoleId = new SelectList(db.AspNetUserRoles, "RoleId", "RoleId");
            return View(Instructor);
        }

        // GET: Students/Create

        public ActionResult Create1()
        {
            var useremail = User.Identity.GetUserName();

            TempData["useremail"] = useremail;
            var userId = User.Identity.GetUserId();
            TempData["userId"] = userId;
            ViewBag.Iid = new SelectList(db.AspNetUserRoles, "UserId", "Notes");

            ViewBag.course_id = new SelectList(db.Courses, "Cid", "Cdesc");
            ViewBag.RoleId = new SelectList(db.AspNetUserRoles, "RoleId", "RoleId");

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

        // POST: Students/Create1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create1([Bind(Include = "Iid,Iaddress,Iemail,Igender,Iofficehours,course_id,con_id,RoleId")] Instructor Instructor)
        {
            if (ModelState.IsValid)
            {
                db.Instructors.Add(Instructor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.course_id = new SelectList(db.Courses, "Cid", "Cdesc", Instructor.course_id);
            ViewBag.RoleId = new SelectList(db.AspNetUserRoles, "RoleId", "RoleId", Instructor.RoleId);
            return View(Instructor);
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
                Instructor Instructor = db.Instructors.Find(id);
                if (Instructor == null)
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