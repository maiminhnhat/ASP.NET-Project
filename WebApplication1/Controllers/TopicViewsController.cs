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
    [Authorize(Roles = "Trainer")]
    public class TopicViewsController : Controller
    {
        private AssignmentDBEntities2 db = new AssignmentDBEntities2();

        // GET: TopicViews
        public ActionResult Index(string searching = "")
        {
            int id = Int32.Parse(Session["account_id"].ToString());
            Trainer trainer = db.Trainers.Where(s => s.Account.ID == id).FirstOrDefault();

            ICollection<Topic> topics = db.Trainers.Where(t => t.ID == trainer.ID).SelectMany(t => t.Topics).Where(t => t.Name.Contains(searching)).ToList();

            return View(topics);
        }

        public ActionResult Edit()
        {
            int id = Int32.Parse(Session["account_id"].ToString());
            Trainer trainer = db.Trainers.Where(s => s.Account.ID == id).FirstOrDefault();

            if (trainer == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = new SelectList(db.Accounts, "ID", "Username", trainer.ID);
            return View(trainer);
        }

        // POST: Staffs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TrainerID,Name,Telephone,Email,Type,Education,Department,ID")] Trainer trainer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trainer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "TopicViews");
            }
            ViewBag.ID = new SelectList(db.Accounts, "ID", "Username", trainer.ID);
            return View(trainer);
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
