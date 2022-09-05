using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using College_Social_Network_Project.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace College_Social_Network_Project.Controllers
{
    public class Join_courseController : Controller
    {
        private Social_NetworkEntities db = new Social_NetworkEntities();

        // GET: Join_course
        public ActionResult Index()
        {
            var join_course = db.Join_course.Include(j => j.AspNetUser).Include(j => j.Course);
            return View(join_course.ToList());
        }

        // GET: Join_course/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Join_course join_course = db.Join_course.Find(id);
            if (join_course == null)
            {
                return HttpNotFound();
            }
            return View(join_course);
        }

        // GET: Join_course/Create
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            var useremail = User.Identity.GetUserName();
            TempData["userId"] = userId;
            TempData["useremail"] = useremail;

            var joining_Courselist = db.Join_course.Where(x => x.Course.Cid == x.Course_id).Where(m => m.User_id == userId).Select(y => y.Course);
            var all = db.Courses;
            var difference = all.Except(joining_Courselist);

            ViewBag.User_id = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.Course_id = new SelectList(difference, "Cid", "Cdesc");
            return View();
        }

        // POST: Join_course/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "User_id,Course_id,Notes")] Join_course join_course)
        {
            if (ModelState.IsValid)
            {
                db.Join_course.Add(join_course);
                db.SaveChanges();
                if (User.IsInRole("Student"))
                    return Redirect("/StudentsWorld/Course");
                if (User.IsInRole("Instructor"))
                    return Redirect("/InstructorWorld/Course");
            }

            ViewBag.User_id = new SelectList(db.AspNetUsers, "Id", "Email", join_course.User_id);
            ViewBag.Course_id = new SelectList(db.Courses, "Cid", "Cdesc", join_course.Course_id);
            return View(join_course);
        }

        // GET: Join_course/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Join_course join_course = db.Join_course.Find(id);
            if (join_course == null)
            {
                return HttpNotFound();
            }
            ViewBag.User_id = new SelectList(db.AspNetUsers, "Id", "Email", join_course.User_id);
            ViewBag.Course_id = new SelectList(db.Courses, "Cid", "Cdesc", join_course.Course_id);
            return View(join_course);
        }

        // POST: Join_course/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "User_id,Course_id,Notes")] Join_course join_course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(join_course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.User_id = new SelectList(db.AspNetUsers, "Id", "Email", join_course.User_id);
            ViewBag.Course_id = new SelectList(db.Courses, "Cid", "Cdesc", join_course.Course_id);
            return View(join_course);
        }

        // GET: Join_course/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Join_course join_course = db.Join_course.Find(id);
            if (join_course == null)
            {
                return HttpNotFound();
            }
            return View(join_course);
        }

        // POST: Join_course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Join_course join_course = db.Join_course.Find(id);
            db.Join_course.Remove(join_course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
