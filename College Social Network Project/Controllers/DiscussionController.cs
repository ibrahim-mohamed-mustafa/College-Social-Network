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

    [Authorize]
    public class DiscussionController : Controller
    {
        private Social_NetworkEntities db = new Social_NetworkEntities();

        // GET: Discussion
        public ActionResult Index(int? page)
        {
            int pagesize = 15, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            string userId = User.Identity.GetUserId();
            TempData["userId"] = userId;
            var dquestions = db.Dquestions.Include(d => d.Dreplays).Include(d => d.AspNetUser);
            TempData.Keep();

            List<Dquestion> li = db.Dquestions.Include(d => d.Dreplays).Include(d => d.AspNetUser).OrderByDescending(a => a.Dquestion_DateTime).ToList();
            IPagedList<Dquestion> dque = li.ToPagedList(pageindex, pagesize);
            return View(dque);


            // return View(dquestions.ToList());
        }

        // GET: Dquestions/CreateQuestion
        public ActionResult CreateQuestion()
        {
            var userId = User.Identity.GetUserId();
            var DateTime = System.DateTime.Now;
            TempData["userIdQuestion"] = userId;
            TempData["datetimeQuestion"] = DateTime;
            TempData.Keep();
            return View();
        }

        // POST: Dquestions/CreateQuestion
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateQuestion([Bind(Include = "Dquestion_id,Dquestion_content,Dquestion_DateTime,Dquestion_FK_User")] Dquestion dquestion)
        {
            var userId = User.Identity.GetUserId();
            var DateTime = System.DateTime.Now;
            TempData["userIdQuestion"] = userId;
            TempData["datetimeQuestion"] = DateTime;
            if (ModelState.IsValid)
            {
                db.Dquestions.Add(dquestion);
                db.SaveChanges();
                TempData.Keep();
                return RedirectToAction("Index");
            }
            return View(dquestion);
        }

        // GET: Dquestions/Edit/5
        public ActionResult EditQuestion(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dquestion dquestion = db.Dquestions.Find(id);
            if (dquestion == null)
            {
                return HttpNotFound();
            }
            ViewBag.Dquestion_FK_User = new SelectList(db.AspNetUsers, "Id", "Email", dquestion.Dquestion_FK_User);
            return View(dquestion);
        }

        // POST: Dquestions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditQuestion([Bind(Include = "Dquestion_id,Dquestion_content,Dquestion_DateTime,Dquestion_FK_User")] Dquestion dquestion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dquestion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Dquestion_FK_User = new SelectList(db.AspNetUsers, "Id", "Email", dquestion.Dquestion_FK_User);
            return View(dquestion);
        }

        // GET: Dquestions/Delete/5
        public ActionResult DeleteQuestion(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dquestion dquestion = db.Dquestions.Find(id);
            if (dquestion == null)
            {
                return HttpNotFound();
            }
            return View(dquestion);
        }

        // POST: Dquestions/Delete/5
        [HttpPost, ActionName("DeleteQuestion")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedQuestion(int id)
        {
            Dquestion dquestion = db.Dquestions.Find(id);
            db.Dquestions.Remove(dquestion);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: Dreplays/Create
        public ActionResult CreateReplay(int? idR)
        {
            var userId = User.Identity.GetUserId();
            var DateTime = System.DateTime.Now;
            TempData["userIdReplay"] = userId;
            TempData["datetimeReplay"] = DateTime;
            TempData["idReplay"] = idR;
            TempData.Keep();

            return View();
        }

        // POST: Dreplays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReplay([Bind(Include = "Dreplay_id,Dreplay_DateTime,Dreplay_FK_User,Dreplay_FK_Dquestion,Dreplay_content")] Dreplay dreplay)
        {
            var userId = User.Identity.GetUserId();
            var DateTime = System.DateTime.Now;

            TempData["userIdReplay"] = userId;
            TempData["datetimeReplay"] = DateTime;

            if (ModelState.IsValid)
            {
                db.Dreplays.Add(dreplay);
                db.SaveChanges();
                TempData.Keep();
                return RedirectToAction("Index");
            }
            return View(dreplay);
        }

        // GET: Dreplays/Edit/5
        public ActionResult EditReplay(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dreplay dreplay = db.Dreplays.Find(id);
            if (dreplay == null)
            {
                return HttpNotFound();
            }
            ViewBag.Dreplay_FK_User = new SelectList(db.AspNetUsers, "Id", "Email", dreplay.Dreplay_FK_User);
            ViewBag.Dreplay_FK_Dquestion = new SelectList(db.Dquestions, "Dquestion_id", "Dquestion_content", dreplay.Dreplay_FK_Dquestion);
            return View(dreplay);
        }

        // POST: Dreplays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditReplay([Bind(Include = "Dreplay_id,Dreplay_DateTime,Dreplay_FK_User,Dreplay_FK_Dquestion,Dreplay_content")] Dreplay dreplay)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dreplay).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Dreplay_FK_User = new SelectList(db.AspNetUsers, "Id", "Email", dreplay.Dreplay_FK_User);
            ViewBag.Dreplay_FK_Dquestion = new SelectList(db.Dquestions, "Dquestion_id", "Dquestion_content", dreplay.Dreplay_FK_Dquestion);
            return View(dreplay);
        }

        // GET: Dreplays/Delete/5
        public ActionResult DeleteReplay(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dreplay dreplay = db.Dreplays.Find(id);
            if (dreplay == null)
            {
                return HttpNotFound();
            }
            return View(dreplay);
        }

        // POST: Dreplays/Delete/5
        [HttpPost, ActionName("DeleteReplay")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedReplay(int id)
        {
            Dreplay dreplay = db.Dreplays.Find(id);
            db.Dreplays.Remove(dreplay);
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