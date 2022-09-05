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
    public class UserRolesController : Controller
    {
        private Social_NetworkEntities db = new Social_NetworkEntities();
       
        // GET: UserRoles
        public ActionResult Index()
        {
            var aspNetUserRoles = db.AspNetUserRoles.Include(a => a.AspNetRole).Include(a => a.AspNetUser).OrderBy(a=>a.AspNetRole.Name);
            return View(aspNetUserRoles.ToList());
        }

       
        // GET: UserRoles/Create
        public ActionResult Create()
        {
            ViewBag.RoleId = new SelectList(db.AspNetRoles, "Id", "Name");
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: UserRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,RoleId,Notes")] AspNetUserRole aspNetUserRole)
        {
            if (ModelState.IsValid)
            {
                db.AspNetUserRoles.Add(aspNetUserRole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoleId = new SelectList(db.AspNetRoles, "Id", "Name", aspNetUserRole.RoleId);
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", aspNetUserRole.UserId);
            return View(aspNetUserRole);
        }

        // GET: UserRoles/Edit/5
            public ActionResult Edit(string Userid, string Roleid)
        {
            if (Userid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUserRole aspNetUserRole = db.AspNetUserRoles.Find(Userid, Roleid);
            if (aspNetUserRole == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleId = new SelectList(db.AspNetRoles, "Id", "Name", aspNetUserRole.RoleId);
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", aspNetUserRole.UserId);
            return View(aspNetUserRole);
        }

        // POST: UserRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,RoleId,Notes")] AspNetUserRole aspNetUserRole)
        {
            if (ModelState.IsValid)
            {
                string ListRoleId = Request["RoleId"];
                string UserId = Request["UserId"];
                string OldRoleId = ListRoleId.ToString();
                DeleteConfirmed1(UserId, OldRoleId);
                db.AspNetUserRoles.Add(aspNetUserRole);
                try
                {
                    db.SaveChanges();
                }
                catch
                {
                    return RedirectToAction("Index");
                }
                db.Entry(aspNetUserRole).State = EntityState.Modified;
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(db.AspNetRoles, "Id", "Name", aspNetUserRole.RoleId);
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", aspNetUserRole.UserId);
            return View(aspNetUserRole);
        }

        // GET: UserRoles/Delete/5
        public ActionResult Delete(string Userid, string Roleid)
        {
            if (Userid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUserRole aspNetUserRole = db.AspNetUserRoles.Find(Userid, Roleid);
            if (aspNetUserRole == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUserRole);
        }

        // POST: UserRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string Userid, string Roleid)
        {
            AspNetUserRole aspNetUserRole = db.AspNetUserRoles.Find( Userid,  Roleid);
            db.AspNetUserRoles.Remove(aspNetUserRole);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public void DeleteConfirmed1(string Userid, string Roleid)
        {
            AspNetUserRole aspNetUserRole = db.AspNetUserRoles.Find( Userid,  Roleid);
            db.AspNetUserRoles.Remove(aspNetUserRole);
            db.SaveChanges();
           // return RedirectToAction("Index");
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
