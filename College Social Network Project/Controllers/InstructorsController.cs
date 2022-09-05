using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using College_Social_Network_Project.Models;

namespace College_Social_Network_Project.Controllers
{
    [Authorize(Roles = "Admin")]
    public class InstructorsController : Controller
    {
        private Social_NetworkEntities db = new Social_NetworkEntities();
       
        // GET: Instructors
        public ActionResult Index()
        {
            var instructors = db.Instructors.Include(i => i.AspNetUserRole).Include(i => i.Course).OrderBy(i => i.Iofficehours); ;
            return View(instructors.ToList());
        }

       
        // GET: Instructors/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            ViewBag.Iid = new SelectList(db.AspNetUserRoles, "UserId", "Notes", instructor.Iid);
           
            ViewBag.course_id = new SelectList(db.Courses, "Cid", "Cdesc", instructor.course_id);
            return View(instructor);
        }

        // POST: Instructors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Iid,Iaddress,Iemail,Igender,Iofficehours,course_id,con_id,RoleId")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(instructor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Iid = new SelectList(db.AspNetUserRoles, "UserId", "Notes", instructor.Iid);
       
            ViewBag.course_id = new SelectList(db.Courses, "Cid", "Cdesc", instructor.course_id);
            return View(instructor);
        }
       
        // GET: Instructors/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Instructor instructor = db.Instructors.Find(id);
            db.Instructors.Remove(instructor);
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
