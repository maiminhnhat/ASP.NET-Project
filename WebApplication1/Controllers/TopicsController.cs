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
    public class TopicsController : Controller
    {
        private ProjectASPDBEntities db = new ProjectASPDBEntities();

        // GET: Topics
        public ActionResult Index(string searching)
        {
            var topic = db.Topics.Include(t => t.Courses);
            return View(db.Topics.Where(x => x.Name.Contains(searching) || searching == null).ToList());
        }

        // GET: Topics/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // GET: Topics/Create
        public ActionResult Create()
        {
            ////ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Name");
            return View();
        }

        // POST: Topics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TopicID,Name,Description,CourseID")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                db.Topics.Add(topic);
                db.SaveChanges();

              

                return RedirectToAction("Index");
            }
            //ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Name", topic.CourseID);
            return View(topic);
        }

        // GET: Topics/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topics.Find(id);

            
            ViewBag.Trainers = new SelectList(db.Trainers, "TrainerID", "Name");
            ViewBag.TrainerTable = db.Topics.Where(t => t.TopicID == id).SelectMany(t => t.Trainers).ToList();

            if (topic == null)
            {
                return HttpNotFound();
            }
            //ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Name", topic.CourseID);
            return View(topic);
        }

        // POST: Topics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]

        //[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TopicID,Name,Description,CourseID")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(topic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ////ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Name", topic.CourseID);
            return View(topic);
        }


        public ActionResult Assign(int TopicID, int Trainers)
        {
            Trainer trainer = db.Trainers.Find(Trainers);
            Topic topic = db.Topics.Find(TopicID);

            if (trainer != null)
            {
                trainer.Topics.Add(topic);
                topic.Trainers.Add(trainer);
                db.SaveChanges();
            }

            return RedirectToAction("Edit", "Topics", new { id = TopicID });
        }
        public ActionResult RemoveTrainer(int trainers , int topicID)
        {
            Trainer trainer = db.Trainers.Find(trainers);
            Topic Topic = db.Topics.Find(topicID);

            trainer.Topics.Remove(Topic);
            Topic.Trainers.Remove(trainer);
            db.SaveChanges();
            return RedirectToAction("Edit", "Topics", new { id = topicID });
        }

        // GET: Topics/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // POST: Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Topic topic = db.Topics.Find(id);
            db.Topics.Remove(topic);
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
