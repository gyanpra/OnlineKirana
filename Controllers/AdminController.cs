using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineKirana;
using System.Data.Entity;
using OnlineKirana.Models;


namespace OnlineKirana.Controllers
{
    public class AdminController : Controller
    {

        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.msg="";
            return View();
        }
        [HttpPost]
        public ActionResult Index(Admin admn)
        {
            OnlineKiranaEntities obj = new OnlineKiranaEntities();
            var a = obj.Admins.Where(l => l.Email.Equals(admn.Email) && l.Password.Equals(admn.Password)).ToList();
            if (a.Count>0)
            {
                Session["Email"] = admn.Email.ToString();
                return RedirectToAction("Create","ManageProducts");

            }
            else
            {
                ViewBag.msg = "Invalid UserName or Password";
            }
            return View();
        }

        public ActionResult DashBoard()
        {
            if (Session["Email"] == null)
            {
                ViewBag.msg = "Log in as Admin to proceed.";
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Admin");
        }

    }
}