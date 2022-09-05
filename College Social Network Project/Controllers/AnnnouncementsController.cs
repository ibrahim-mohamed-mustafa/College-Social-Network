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
using PagedList;

namespace College_Social_Network_Project.Controllers
{
    public class AnnnouncementsController : Controller
    {

        // List<tbl_categroy> li = db.tbl_categroy.OrderByDescending(x => x.cat_id).ToList();
        private Social_NetworkEntities db = new Social_NetworkEntities();
        [Authorize(Roles = "Instructor")]
        // GET: Annnouncements
        public ActionResult Index(int? course_id, int? page)
        {
            if (course_id == null)
            {
                course_id = Convert.ToInt32(TempData["course_id"]);
            }

            int pagesize = 10, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var userId = User.Identity.GetUserId();
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(userId);
            if (instructor == null)
            {
                return RedirectToAction("Create1", "InstructorWorld");
            }
            TempData["course_id"] = course_id;
            List<Annnouncement> liann = (from m in db.Course_Ann

                                         where m.Course_id == course_id
                                         where m.Annnouncement.Announce_FK_INS == userId
                                         orderby m.Annnouncement.Announce_DateTime
                                         descending
                                         select m.Annnouncement).ToList();
            //List<Annnouncement> li = db.Annnouncements.Include(a => a.Instructor).Where(x => x.Announce_FK_INS == userId).OrderByDescending(a => a.Announce_DateTime).ToList();
            IPagedList<Annnouncement> stu = liann.ToPagedList(pageindex, pagesize);
            TempData.Keep();
            return View(stu);
        }
        [Authorize(Roles = "Student")]
        // GET: Annnouncements
        public ActionResult student(int? course_id, int? page)
        {
            int pagesize = 10, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var userId = User.Identity.GetUserId();
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(userId);
            if (student == null)
            {
                return RedirectToAction("Create1", "StudentsWorld");
            }
            List<Annnouncement> liann = (from m in db.Course_Ann

                                         where m.Course_id == course_id

                                         orderby m.Annnouncement.Announce_DateTime
                                         descending
                                         select m.Annnouncement).ToList();
            IPagedList<Annnouncement> stu = liann.ToPagedList(pageindex, pagesize);
            return View(stu);
        }
        [Authorize(Roles = "Instructor")]
        // GET: Annnouncements/Create
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            var DateTime = System.DateTime.Now;
            TempData["userId"] = userId;
            TempData["datetime"] = DateTime;
            ViewBag.Announce_FK_INS = new SelectList(db.Instructors, "Iid", "Iemail");
            TempData.Keep();
            return View();
        }

        // POST: Annnouncements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Announce_id,Announce_content,Announce_DateTime,Announce_FK_INS")] Annnouncement annnouncement)
        {
            int course_id = Convert.ToInt32(TempData["course_id"]);
            var userId = User.Identity.GetUserId();
            var DateTime = System.DateTime.Now;
            TempData["userId"] = userId;
            TempData["datetime"] = DateTime;

            Course_Ann course_Ann = new Course_Ann();
            course_Ann.ann_id = annnouncement.Announce_id;
            course_Ann.Course_id = course_id;

            if (ModelState.IsValid)
            {
                db.Course_Ann.Add(course_Ann);
                db.Annnouncements.Add(annnouncement);
                db.SaveChanges();
                TempData.Keep();
                return RedirectToAction("Index", course_id);
            }

            ViewBag.Announce_FK_INS = new SelectList(db.Instructors, "Iid", "Iemail", annnouncement.Announce_FK_INS);
            TempData.Keep();
            return View(annnouncement);
        }
        [Authorize(Roles = "Instructor")]
        // GET: Annnouncements/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Annnouncement annnouncement = db.Annnouncements.Find(id);
            if (annnouncement == null)
            {
                return HttpNotFound();
            }
            ViewBag.Announce_FK_INS = new SelectList(db.Instructors, "Iid", "Iemail", annnouncement.Announce_FK_INS);

            return View(annnouncement);
        }

        // POST: Annnouncements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Announce_id,Announce_content,Announce_DateTime,Announce_FK_INS")] Annnouncement annnouncement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(annnouncement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Announce_FK_INS = new SelectList(db.Instructors, "Iid", "Iemail", annnouncement.Announce_FK_INS);

            return View(annnouncement);
        }
        [Authorize(Roles = "Instructor")]
        // GET: Annnouncements/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Annnouncement annnouncement = db.Annnouncements.Find(id);
            if (annnouncement == null)
            {
                return HttpNotFound();
            }
            return View(annnouncement);
        }

        // POST: Annnouncements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            int course_id = Convert.ToInt32(TempData["course_id"]);
            Course_Ann course_Ann = db.Course_Ann.Find(course_id, id);
            Annnouncement annnouncement = db.Annnouncements.Find(id);
            db.Course_Ann.Remove(course_Ann);
            db.Annnouncements.Remove(annnouncement);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        public ActionResult Course()
        {

            var id = User.Identity.GetUserId();

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

        public ActionResult Course_student()
        {

            var id = User.Identity.GetUserId();

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
