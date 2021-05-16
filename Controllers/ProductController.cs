using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using OnlineKirana.Models;

namespace OnlineKirana.Controllers
{
    public class ProductController : Controller
    {
        private OnlineKiranaEntities db = new OnlineKiranaEntities();
        // GET: Product
        public ActionResult Index(string SearchBy, string search)
        {
            if (SearchBy == "Brand")
            {
                return View(db.Products.Where(x => x.Brand == search || search == null).ToList());
            }
            else if (SearchBy == "Category")
            {
                return View(db.Products.Where(x => x.Category == search || search == null).ToList());
            }
            else
            {
                return View(db.Products.Where(x => x.ProductName.StartsWith(search) || search == null).ToList());
            }
        }
    }
}