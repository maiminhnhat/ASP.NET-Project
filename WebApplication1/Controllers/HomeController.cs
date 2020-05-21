using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login(){
            return View();
        }
        [HttpPost]
        public ActionResult Login(Account model, string returnUrl)
        {
            AssignmentDBEntities2 db = new AssignmentDBEntities2();
            var dataItem = db.Accounts.Where(x => x.Username == model.Username && x.PassWord == model.PassWord).FirstOrDefault();
          
            if (dataItem != null)
            {
                FormsAuthentication.SetAuthCookie(dataItem.Username, false);
             
                if(dataItem != null)
                {
                    Session["account_id"] = dataItem.ID;
                }
                else
                {
                    return View();
                }

                if (dataItem.Role == "Admin")
                {
                    return RedirectToAction("Index","Accounts");
                }
                else if (dataItem.Role == "Staff")
                {
                    
                    return RedirectToAction("Index","Trainees");
                }
                else if (dataItem.Role == "Trainer")
                {
                    return RedirectToAction("Index", "TopicViews");
                }
                else if (dataItem.Role == "Trainee")
                {
                    return RedirectToAction("Index", "Traineesview");
                }

                else
                {
                    ModelState.AddModelError("", "Invalid user/pass");
                    return View();
                }

            }

            return View();
        }
        [Authorize]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }
        






    }
}