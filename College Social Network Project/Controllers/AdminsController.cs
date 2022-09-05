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
    public class AdminsController : Controller
    {
        private Social_NetworkEntities db = new Social_NetworkEntities();

        // GET: Admins
        public ActionResult Index()
        {
            var admins = db.Admins.Include(a => a.Course).Include(a => a.AspNetUserRole);
            return View(admins.ToList());
        }

     
        // GET: Admins/Create
        public ActionResult Create()
        {


            var userId = User.Identity.GetUserId();
            var useremail = User.Identity.GetUserName();
            TempData["userId"] = userId;
            TempData["useremail"] = useremail;
            var id = userId;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return RedirectToAction("Create1");

                // return HttpNotFound();
            }

            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", admin.Aid);
            ViewBag.course_id = new SelectList(db.Courses, "Cid", "Cdesc");
            ViewBag.Aid = new SelectList(db.AspNetUserRoles, "UserId", "Notes");
            return View(admin);
        }

        // POST: Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Aid,Aemail,Agender,Aaddress,course_id,RoleId")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                //db.Admins.Add(admin);
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Dashboard");
            }

           
            ViewBag.Aid = new SelectList(db.AspNetUserRoles, "UserId", "Notes", admin.Aid);
            return View(admin);
        }


        public ActionResult Create1()
        {
            var useremail = User.Identity.GetUserName();

            TempData["useremail"] = useremail;
            var userId = User.Identity.GetUserId();
            TempData["userId"] = userId;
            
            return View();

        }

        // POST: Admins/Create1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create1([Bind(Include = "Aid,Aemail,Agender,Aaddress,course_id,RoleId")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Admins.Add(admin);
                db.SaveChanges();
                
                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.course_id = new SelectList(db.Courses, "Cid", "Cdesc", admin.course_id);
            ViewBag.Aid = new SelectList(db.AspNetUserRoles, "UserId", "Notes", admin.Aid);
            
            return View(admin);
        }


        // GET: Admins/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            ViewBag.course_id = new SelectList(db.Courses, "Cid", "Cdesc", admin.course_id);
            ViewBag.Aid = new SelectList(db.AspNetUserRoles, "UserId", "Notes", admin.Aid);
            return View(admin);
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Aid,Aemail,Agender,Aaddress,course_id,RoleId")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.course_id = new SelectList(db.Courses, "Cid", "Cdesc", admin.course_id);
            ViewBag.Aid = new SelectList(db.AspNetUserRoles, "UserId", "Notes", admin.Aid);
            return View(admin);
        }

        // GET: Admins/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Admin admin = db.Admins.Find(id);
            db.Admins.Remove(admin);
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
