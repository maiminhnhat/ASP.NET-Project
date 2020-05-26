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
    [Authorize(Roles = "Trainee")]
    public class TraineesviewController : Controller
    {
        private ProjectASPDBEntities db = new ProjectASPDBEntities();

        // GET: Traineesview
        public ActionResult Index(string searching = "")
        {
            int id = Int32.Parse(Session["account_id"].ToString());
            Trainee trainees = db.Trainees.Where(t => t.Account.ID == id).FirstOrDefault();
          


            ICollection<Course> courses = db.Trainees.Where(t => t.ID ==  trainees.ID).SelectMany(t => t.Courses).Where(t => t.Name.Contains(searching)).ToList();
            return View(courses);
        }

        
        // GET: Traineesview/Edit/5
        public ActionResult Edit()
        {
         
            int id = Int32.Parse(Session["account_id"].ToString());
            Trainee trainee = db.Trainees.Where(s => s.Account.ID == id).FirstOrDefault();
            if (trainee == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = new SelectList(db.Accounts, "ID", "Username", trainee.ID);
            ViewBag.TrainerID = new SelectList(db.Trainers, "TrainerID", "Name", trainee.TrainerID);
            return View(trainee);
        }

        // POST: Traineesview/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TraineeID,Name,Age,Email,DoB,Education,PRL,TOEIC,Address,Department,ID,TrainerID")] Trainee trainee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trainee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID = new SelectList(db.Accounts, "ID", "Username", trainee.ID);
            ViewBag.TrainerID = new SelectList(db.Trainers, "TrainerID", "Name", trainee.TrainerID);
            return View(trainee);
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
