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
    
    public class QuizController : Controller
    {
        private Social_NetworkEntities db = new Social_NetworkEntities();
        [Authorize(Roles = "Student")]
        public ActionResult Index_stu()
        {
            var id = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return RedirectToAction("Create1", "StudentsWorld");
            }
            
            return View();
        }

        [Authorize(Roles = "Instructor")]
        // GET: Quiz/Instructor
        public ActionResult Index()
        {
            var id = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor Instructor = db.Instructors.Find(id);
            if (Instructor == null)
            {
                return RedirectToAction("Create1", "InstructorWorld");
            }
          
            return View();
        }

        // GET: TBL_QUESTIONS/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewData["Cat_id"] = TempData["Cat_id"];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_QUESTIONS tBL_QUESTIONS = db.TBL_QUESTIONS.Find(id);
            if (tBL_QUESTIONS == null)
            {
                return HttpNotFound();
            }
            ViewBag.q_fk_catid = new SelectList(db.tbl_categroy, "cat_id", "cat_name", tBL_QUESTIONS.q_fk_catid);
            TempData.Keep();

            return View(tBL_QUESTIONS);
        }

        // POST: TBL_QUESTIONS/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QUESTION_ID,Q_TEXT,OPA,OPB,OPC,OPD,COP,q_fk_catid")] TBL_QUESTIONS tBL_QUESTIONS)
        {

            int Cat_id = Convert.ToInt32(TempData["Cat_id"]);
            if (ModelState.IsValid)
            {
                db.Entry(tBL_QUESTIONS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewAllQuestions", Cat_id);
            }
            ViewBag.q_fk_catid = new SelectList(db.tbl_categroy, "cat_id", "cat_name", tBL_QUESTIONS.q_fk_catid);
            TempData.Keep();
            return View(tBL_QUESTIONS);
        }

        // GET: TBL_QUESTIONS/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewData["Cat_id"] = TempData["Cat_id"];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_QUESTIONS tBL_QUESTIONS = db.TBL_QUESTIONS.Find(id);
            if (tBL_QUESTIONS == null)
            {
                return HttpNotFound();
            }
            TempData.Keep();
            return View(tBL_QUESTIONS);
        }

        // POST: TBL_QUESTIONS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            int Cat_id = Convert.ToInt32(TempData["Cat_id"]);
            TBL_QUESTIONS tBL_QUESTIONS = db.TBL_QUESTIONS.Find(id);
            db.TBL_QUESTIONS.Remove(tBL_QUESTIONS);
            db.SaveChanges();
            TempData.Keep();
            return RedirectToAction("ViewAllQuestions", Cat_id);
        }

        // GET: tbl_categroy/Edit/5
        public ActionResult Edit_Cat(int? id)
        {
            ViewData["course_id"] = TempData["course_id"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_categroy tbl_categroy = db.tbl_categroy.Find(id);
            if (tbl_categroy == null)
            {
                return HttpNotFound();
            }
            TempData.Keep();

            return View(tbl_categroy);
        }

        // POST: tbl_categroy/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit_Cat([Bind(Include = "cat_id,cat_name,cat_fk_adid,cat_encyptedstring,cat_time")] tbl_categroy tbl_categroy)
        {
            int course_id = Convert.ToInt32(TempData["course_id"]);
            if (ModelState.IsValid)
            {
                db.Entry(tbl_categroy).State = EntityState.Modified;
                db.SaveChanges();
                TempData.Keep();
                return RedirectToAction("MainAddcategory", course_id);
            }

            TempData.Keep();
            return View(tbl_categroy);
        }

        // GET: tbl_categroy/Delete/5
        public ActionResult Delete_Cat(int? id)
        {
            ViewData["course_id"] = TempData["course_id"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_categroy tbl_categroy = db.tbl_categroy.Find(id);
            if (tbl_categroy == null)
            {
                return HttpNotFound();
            }
            TempData.Keep();

            return View(tbl_categroy);
        }

        // POST: tbl_categroy/Delete/5
        [HttpPost, ActionName("Delete_Cat")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed_Cat(int id)
        {
            int course_id = Convert.ToInt32(TempData["course_id"]);
            Course_Cat course_Cat = db.Course_Cat.Find(course_id,id);
            tbl_categroy tbl_categroy = db.tbl_categroy.Find(id);
            db.Course_Cat.Remove(course_Cat);
            db.tbl_categroy.Remove(tbl_categroy);
            db.SaveChanges();
            TempData.Keep();
            return RedirectToAction("MainAddcategory", course_id);
        }

        [Authorize(Roles = "Student")]
        public ActionResult Course_Report_Stu()
        {
            var id = User.Identity.GetUserId();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return RedirectToAction("Create1", "StudentsWorld");
            }

            var list = from m in db.Join_course
                       where m.User_id == id
                       select m.Course;
            return View(list);

        }

        [Authorize(Roles = "Student")]
        public ActionResult Course_cat_Report_Stu  (int? course_id)
        {
            var id = User.Identity.GetUserId();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return RedirectToAction("Create1", "StudentsWorld");
            }

            List<tbl_categroy> list = (from m in db.Course_Cat
                                       where m.Course_id == course_id
                                       orderby m.tbl_categroy.cat_id
                                       select m.tbl_categroy).ToList();

            TempData["course_idR"] = course_id;
            TempData.Keep();
            ViewData["list"] = list;
            return View();
        }

        [Authorize(Roles = "Student")]
        public ActionResult Reports_Stu(int? course_id, int? Cat_id, int? page)
        {
            var userId = User.Identity.GetUserId();
            int pagesize = 15, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            List<tbl_categroy> licat = (from m in db.Course_Cat
                                        where m.Course_id == course_id
                                        where m.Cat_id == Cat_id
                                        orderby m.tbl_categroy.cat_id
                                        select m.tbl_categroy).ToList();

            List<tbl_categroy> licat1 = db.tbl_categroy.Where(x => x.cat_fk_adid == userId).ToList();
            List<TBL_SETEXAM> li = db.TBL_SETEXAM.Where(x => x.EXAM_FK_STU == userId).ToList();
            List<TBL_SETEXAM> newli = new List<TBL_SETEXAM>();
            foreach (var item in licat)
            {
                foreach (var ex in li)
                {
                    if (item.cat_id == ex.EXAM_FK_CATID)
                    {
                        newli.Add(ex);
                    }

                }
            }
            IPagedList<TBL_SETEXAM> stu = newli.ToPagedList(pageindex, pagesize);
            return View(stu);
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
                return RedirectToAction("Create1", "InstructorWorld");
            }

            var list = from m in db.Join_course
                       where m.User_id == id
                       select m.Course;
            return View(list);

        }

        public ActionResult Course_Questions()
        {
            var id = User.Identity.GetUserId();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor Instructor = db.Instructors.Find(id);
            if (Instructor == null)
            {
                return RedirectToAction("Create1", "InstructorWorld");
            }

            var list = from m in db.Join_course
                       where m.User_id == id
                       select m.Course;
            return View(list);

        }

        public ActionResult Course_Report()
        {
            var id = User.Identity.GetUserId();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor Instructor = db.Instructors.Find(id);
            if (Instructor == null)
            {
                return RedirectToAction("Create1" , "InstructorWorld");
            }

            var list = from m in db.Join_course
                       where m.User_id == id
                       select m.Course;
            return View(list);

        }

        public ActionResult Course_cat_Report(int? course_id)
        {
            var id = User.Identity.GetUserId();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor Instructor = db.Instructors.Find(id);
            if (Instructor == null)
            {
                return RedirectToAction("Create1", "InstructorWorld");
            }

            List<tbl_categroy> list = (from m in db.Course_Cat
                                       where m.Course_id == course_id
                                       orderby m.tbl_categroy.cat_id
                                       select m.tbl_categroy).ToList();
            TempData["course_idR"] = course_id;
            TempData.Keep();
            ViewData["list"] = list;
            return View();
        }


        [Authorize(Roles = "Instructor")]
        [HttpGet]
        public ActionResult MainAddcategory(int? course_id)
        {
            if (course_id == null)
            {
                course_id = Convert.ToInt32(TempData["course_id"]);
            }

            var userId = User.Identity.GetUserId();
            List<tbl_categroy> list = (from m in db.Course_Cat
                                       where m.Course_id == course_id
                                       where m.tbl_categroy.cat_fk_adid == userId
                                       orderby m.tbl_categroy.cat_id
                                       select m.tbl_categroy).ToList();
            TempData["course_id"] = course_id;

            ViewData["list"] = list;
            TempData.Keep();
            return View();

        }

        [HttpPost]
        public ActionResult MainAddcategory(tbl_categroy cat)
        {
            int course_id = Convert.ToInt32(TempData["course_id"]);

            List<tbl_categroy> li = db.tbl_categroy.OrderByDescending(x => x.cat_id).ToList();
            ViewData["list"] = li;
            Random r = new Random();
            tbl_categroy c = new tbl_categroy();
            c.cat_name = cat.cat_name;
            c.cat_time = cat.cat_time;
            c.cat_encyptedstring = cyptop.Encrypt(cat.cat_name.Trim() + r.Next().ToString(), true);
            var userId = User.Identity.GetUserId();
            c.cat_fk_adid = userId;
            db.tbl_categroy.Add(c);
            Course_Cat course_Cat = new Course_Cat();
            course_Cat.Cat_id = cat.cat_id;
            course_Cat.Course_id = course_id;
            db.Course_Cat.Add(course_Cat);

            db.SaveChanges();
            TempData.Keep();
            return RedirectToAction("MainAddcategory", course_id);
        }

        public ActionResult ViewAllQuestions(int? Cat_id, int? page)
        {


            if (Cat_id == null)
            {
                Cat_id = Convert.ToInt32(TempData["Cat_id"]);
            }
            TempData["Cat_id"] = Cat_id;
           
            var marks = 0;
            int pagesize = 15, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            List<TBL_QUESTIONS> li = db.TBL_QUESTIONS.Where(x => x.q_fk_catid == Cat_id).ToList();
            foreach (var a in li)
            {
                marks = (int)(marks +a.Mark);
            }
            TempData["marks"] = marks;
            IPagedList<TBL_QUESTIONS> stu = li.ToPagedList(pageindex, pagesize);
            TempData.Keep();
            return View(stu);
        }

        [Authorize(Roles = "Instructor")]
        public ActionResult Reports(int? course_id, int? Cat_id, int? page)
        {
            var userId = User.Identity.GetUserId();
            int pagesize = 15, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            List<tbl_categroy> licat = (from m in db.Course_Cat
                                        where m.Course_id == course_id
                                        where m.Cat_id == Cat_id
                                        where m.tbl_categroy.cat_fk_adid == userId
                                        orderby m.tbl_categroy.cat_id
                                        select m.tbl_categroy).ToList();

            List<tbl_categroy> licat1 = db.tbl_categroy.Where(x => x.cat_fk_adid == userId).ToList();
            List<TBL_SETEXAM> li = db.TBL_SETEXAM.Where(x => x.EXAM_FK_STU == x.Student.Sid).ToList();
            List<TBL_SETEXAM> newli = new List<TBL_SETEXAM>();
            foreach (var item in licat)
            {
                foreach (var ex in li)
                {
                    if (item.cat_id == ex.EXAM_FK_CATID)
                    {
                        newli.Add(ex);
                    }

                }
            }
            IPagedList<TBL_SETEXAM> stu = newli.ToPagedList(pageindex, pagesize);
            return View(stu);
        }

        // GET: Addquestion
        [Authorize(Roles = "Instructor")]
        [HttpGet]
        public ActionResult Addquestion(int? course_id)
        {
            TempData["msg"] = "";
            if (course_id == null)
            {
                course_id = Convert.ToInt32(TempData["course_idQ"]);
            }
            var userId = User.Identity.GetUserId();
            TempData["course_idQ"] = course_id;


            List<tbl_categroy> list = (from m in db.Course_Cat
                                       where m.Course_id == course_id
                                       where m.tbl_categroy.cat_fk_adid == userId
                                       orderby m.tbl_categroy.cat_id
                                       select m.tbl_categroy).ToList();
            ViewBag.list = new SelectList(list, "cat_id", "cat_name");
            ViewBag.Cop = new SelectList(list, "", "cat_name");
            TempData.Keep();
            return View();
        }

        // Post: Addquestion
        [HttpPost]
        public ActionResult Addquestion(TBL_QUESTIONS q)
        {
            //int sid = Convert.ToInt32(Session["ad_id"]);
            int course_id = Convert.ToInt32(TempData["course_idQ"]);
            var userId = User.Identity.GetUserId();
            List<tbl_categroy> li = db.tbl_categroy.Where(x => x.cat_fk_adid == userId).ToList();
            ViewBag.list = new SelectList(li, "cat_id", "cat_name");
            TBL_QUESTIONS QA = new TBL_QUESTIONS();
            QA.Q_TEXT = q.Q_TEXT;
            QA.OPA = q.OPA;
            QA.OPB = q.OPB;
            QA.OPC = q.OPC;
            QA.OPD = q.OPD;
            QA.COP = q.COP;
            QA.q_fk_catid = q.q_fk_catid;
            QA.Mark = q.Mark;
            db.TBL_QUESTIONS.Add(QA);
            db.SaveChanges();
            TempData["msg"] = "Question added successfully.";
            ViewData["msg"] = "Question added successfully.";
            TempData.Keep();
            return RedirectToAction("Addquestion", course_id);
        }

        [Authorize(Roles = "Student")]
        public ActionResult StudentExam()
        {
            var id = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return RedirectToAction("Create1", "StudentsWorld");
            }
            return View();
        }

        [HttpPost]
        public ActionResult StudentExam(string room)
        {
            var userId = User.Identity.GetUserId();
            bool ch = true;
            List<tbl_categroy> list = db.tbl_categroy.ToList();
            List<TBL_SETEXAM> liststu = db.TBL_SETEXAM.ToList();
            foreach (var item in list)
            {
                if (item.cat_encyptedstring == room)
                {
                    foreach (var stu in liststu)
                    {
                        if (stu.EXAM_FK_STU == userId && stu.EXAM_FK_CATID == item.cat_id)
                        {
                            ch = false;
                            break;
                        }
                    }
                    TempData["Fullmark"] = 0;
                    if (ch == true)
                    {
                        List<TBL_QUESTIONS> li = db.TBL_QUESTIONS.Where(x => x.q_fk_catid == item.cat_id).ToList();
                        Queue<TBL_QUESTIONS> queue = new Queue<TBL_QUESTIONS>();
                        foreach (TBL_QUESTIONS a in li)
                        {
                            queue.Enqueue(a);
                            TempData["Fullmark"] = Convert.ToInt32(TempData["Fullmark"]) + a.Mark;

                        }
                        TempData["catename"] = item.cat_name;
                        TempData["examTime"] = item.cat_time;
                        TempData["examid"] = item.cat_id;
                        TempData["questions"] = queue;
                        TempData["score"] = 0;

                        TempData["TempQueston"] = 1;
                        TempData["CountQuestion"] = queue.Count();


                        TempData.Keep();
                        return RedirectToAction("QuizStart");
                    }
                }
                else
                {
                    if (ch == false)
                    {
                        ViewBag.error = "Exam For Onetime Only";
                    }
                    else
                    {
                        ViewBag.error = "No Room Found";
                    }

                }
            }
            return View();
        }

        [Authorize(Roles = "Student")]
        public ActionResult QuizStart()
        {
            TBL_QUESTIONS q = null;
            if (TempData["questions"] != null)
            {
                Queue<TBL_QUESTIONS> qlist = (Queue<TBL_QUESTIONS>)TempData["questions"];
                if (qlist.Count > 0)
                {
                    q = qlist.Peek();
                    qlist.Dequeue();
                    TempData["CurrentQ"] = q.COP;
                    TempData["questions"] = qlist;
                    TempData["CurrentMark"] = q.Mark;
                    TempData.Keep();
                }
                else
                {
                    return RedirectToAction("EndExam");
                }
            }
            else
            {
                return RedirectToAction("StudentExam");
            }
            TempData["mark"] = q.Mark;



            int timer = Convert.ToInt32(TempData["examTime"]);
            if (Session["Rem_Time"] == null)
            {
                Session["Rem_Time"] = DateTime.Now.AddMinutes(timer).ToString("dd-MM-yyyy h:mm:ss tt");
            }
            ViewBag.Rem_Time = Session["Rem_Time"];
            TempData.Keep();

            return View(q);
        }

        [HttpPost]
        public ActionResult QuizStart(TBL_QUESTIONS q)
        {
            var c = TempData["CurrentQ"];

            if (q.OPA == (string)c)
            {
                TempData["score"] = Convert.ToInt32(TempData["score"]) + Convert.ToInt32(TempData["mark"]);
                TempData["TempQueston"] = Convert.ToInt32(TempData["TempQueston"]) + 1;

            }
            //TempData["Fullmark"] = Convert.ToInt32(TempData["Fullmark"]) + Convert.ToInt32(TempData["mark"]);
            TempData.Keep();
            return RedirectToAction("QuizStart");
        }

        [Authorize(Roles = "Student")]
        public ActionResult EndExam(TBL_QUESTIONS q)
        {
            var userId = User.Identity.GetUserId();
            TBL_SETEXAM ex = new TBL_SETEXAM();
            ex.EXAM_FK_CATID = Convert.ToInt32(TempData["examid"]);
            ex.EXAM_FK_STU = userId;
            ex.EXAM_DATE = System.DateTime.Now;
            ex.EXAM_STD_SCORE = Convert.ToInt32(TempData["score"]);
            ex.EXAM_NAME = TempData["catename"].ToString();
            ex.EXAM_FullMark = Convert.ToInt32(TempData["Fullmark"]);
            db.TBL_SETEXAM.Add(ex);
            db.SaveChanges();

            return View();
        }

    }


}