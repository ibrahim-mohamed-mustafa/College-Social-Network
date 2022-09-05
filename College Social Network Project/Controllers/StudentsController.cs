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
    public class StudentsController : Controller
    {
        private Social_NetworkEntities db = new Social_NetworkEntities();
        
        // GET: Students
        public ActionResult Index()
        {
            var students = db.Students.Include(s => s.AspNetUserRole).Include(s => s.Course).OrderBy(s=>s.Sacademicyear);
            return View(students.ToList());
        }

      
        // GET: Students/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.Sid = new SelectList(db.AspNetUserRoles, "UserId", "Notes", student.Sid);
            
            ViewBag.course_id = new SelectList(db.Courses, "Cid", "Cdesc", student.course_id);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Sid,Saddress,Semail,Sgender,Sacademicyear,course_id,con_id,RoleId")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Sid = new SelectList(db.AspNetUserRoles, "UserId", "Notes", student.Sid);
         
            ViewBag.course_id = new SelectList(db.Courses, "Cid", "Cdesc", student.course_id);
            return View(student);
        }
        
        // GET: Students/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
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
