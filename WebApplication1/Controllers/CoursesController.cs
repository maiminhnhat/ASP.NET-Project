using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Staff")]
    public class CoursesController : Controller
    {
        private ProjectASPDBEntities db = new ProjectASPDBEntities();

        // GET: Courses
        public ActionResult Index(string searching)
        {
            var courses = db.Courses.Include(c => c.Category);
            return View(db.Courses.Where(x => x.Name.Contains(searching) || searching == null).ToList());
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            ViewBag.CateID = new SelectList(db.Categories, "CateID", "Name");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseID,Name,Description,CateID")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CateID = new SelectList(db.Categories, "CateID", "Name", course.CateID);
            return View(course);
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            ViewBag.Topics = new SelectList(db.Topics, "TopicID", "Name");
            ViewBag.TopicTable = db.Courses.Where(t => t.CourseID == id).SelectMany(t => t.Topics).ToList();
            ViewBag.Trainees = new SelectList(db.Trainees, "TraineeID", "Name");
            ViewBag.TraineeTable = db.Courses.Where(t => t.CourseID == id).SelectMany(t => t.Trainees).ToList();
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.CateID = new SelectList(db.Categories, "CateID", "Name", course.CateID);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseID,Name,Description,CateID")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CateID = new SelectList(db.Categories, "CateID", "Name", course.CateID);
            return View(course);
        }

        public ActionResult Assign (int CourseID, int Topics)
        {
            Course Course = db.Courses.Find(CourseID);
            Topic Topic = db.Topics.Find(Topics);

            if (Topic != null)
            {
                Topic.Courses.Add(Course);
                Course.Topics.Add(Topic);
                db.SaveChanges();
            }
            return RedirectToAction("Edit", "Courses", new { id = CourseID });
        }
        public ActionResult RemoveTopic(int courseID, int topic)
        {
            Topic Topic = db.Topics.Find(topic);
            Course Course = db.Courses.Find(courseID);

            Course.Topics.Remove(Topic);
            Topic.Courses.Remove(Course);
            db.SaveChanges();

            return RedirectToAction("Edit", "Courses", new { id = courseID });

        }

        public ActionResult AssignTrainee(int CourseID, int Trainees)
        {
            Course Course = db.Courses.Find(CourseID);
            Trainee Trainee = db.Trainees.Find(Trainees);

            if (Trainee != null)
            {
                Course.Trainees.Add(Trainee);
                Trainee.Courses.Add(Course);
                db.SaveChanges();

            }
            return RedirectToAction("Edit", "Courses", new { id = CourseID });

        }

        public ActionResult RemoveTrainee(int courseID, int trainee)
        {
            Trainee Trainee = db.Trainees.Find(trainee);
            Course Course = db.Courses.Find(courseID);

            Course.Trainees.Remove(Trainee);
            Trainee.Courses.Remove(Course);
            db.SaveChanges();

            return RedirectToAction("Edit", "Courses", new { id = courseID });

        }


        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
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
