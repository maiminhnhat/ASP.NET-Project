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
    public class StaffsController : Controller
    {
        private ProjectASPDBEntities db = new ProjectASPDBEntities();



        // GET: Staffs/Edit/5
        public ActionResult Edit()
        {
            int id = Int32.Parse(Session["account_id"].ToString());
            Staff staff = db.Staffs.Where(s => s.Account.ID == id).FirstOrDefault();

            if (staff == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = new SelectList(db.Accounts, "ID", "Username", staff.ID);
            return View(staff);
        }

        // POST: Staffs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StaffID,Name,Telephone,Email,DoB,ID")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staff).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index","Trainees");
            }
            ViewBag.ID = new SelectList(db.Accounts, "ID", "Username", staff.ID);
            return View(staff);
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
