using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using OnlineKirana.Models;
using System.Data;
using System.Data.SqlClient;

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

        public ActionResult Adtocart(int? Id)
        {

            Product p = db.Products.Where(x => x.ProductID == Id).SingleOrDefault();
            return View(p);
        }


        List<cart> li = new List<cart>();
        [HttpPost]
        //removed tbl_product pi
        public ActionResult Adtocart(Product pi, string qty, int Id)
        {
            Product p = db.Products.Where(x => x.ProductID == Id).SingleOrDefault();

            cart c = new cart();
            c.productid = p.ProductID;
            c.price = p.Price;
            c.qty = Convert.ToInt32(qty);
            c.bill = c.price * c.qty;
            c.productname = p.ProductName;
            if (TempData["cart"] == null)
            {
                li.Add(c);
                TempData["cart"] = li;

            }
            else
            {
                List<cart> li2 = TempData["cart"] as List<cart>;
                int flag = 0;
                foreach (var item in li2)
                {
                    if (item.productid == c.productid)
                    {
                        item.qty += c.qty;
                        item.bill += c.bill;
                        flag = 1;

                    }

                }
                if (flag == 0)
                {
                    li2.Add(c);
                }

                TempData["cart"] = li2;
            }

            TempData.Keep();




            return RedirectToAction("Index2");
        }

        

        public ActionResult Index2(string SearchBy, string search)
        {
            //Session["u_id"] = 2;
            if (TempData["cart"] != null)
            {
                float x = 0;
                List<cart> li2 = TempData["cart"] as List<cart>;
                foreach (var item in li2)
                {
                    x += item.bill;

                }

                TempData["total"] = x;
            }
            TempData.Keep();
            //return View(db.Products.OrderByDescending(x => x.ProductID).ToList());
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

        public ActionResult checkout()
        {

            return View((List<Product>)Session["cart"]);

        }

        /*public ActionResult checkout(OrderDetail o)
        {

            List<cart> li = TempData["cart"] as List<cart>;
            OrderMaster inv = new OrderMaster();
            //inv.CustomerID = Convert.ToInt32(Session["u_id"].ToString());
            inv.OrderDate = System.DateTime.Now;
            //inv.TotalAmount = Convert.ToInt32(TempData["total"].ToString());
            db.OrderMasters.Add(inv);
            //db.SaveChanges();
            foreach (var item in li)
            {
                OrderDetail od = new OrderDetail();
                od.ProductID = item.productid;
                //od.o_fk_invoice = inv.in_id;
                //od. = System.DateTime.Now;
                od.Quantity = item.qty;
                od.Amount = (int)item.price;
                //od.Price = item.price;
                db.OrderDetails.Add(od);
                //db.SaveChanges();


            }
            TempData.Remove("total");
            TempData.Remove("cart");
            TempData["msg"] = "Transaction has been completed";
            TempData.Keep();
            return RedirectToAction("Index");
        }
        */

        /*[HttpDelete]
       public ActionResult Remove(int? id)
        {
            List<cart> li = TempData["cart"] as List<cart>;
            cart c = li.Where(item => item.productid == id).SingleOrDefault();
            li.Remove(c);
            float h = 0;
            foreach (var item in li)
            {
                h += item.bill;

            }
            TempData["total"] = h;
            TempData.Keep();
            return RedirectToAction("checkout");

        }*/
        
        public ActionResult Remove(int? id)
        {
            List<Product> cart = (List<Product>)Session["cart"];
            foreach (var item in cart)
            {
                if (item.ProductID == id)
                {
                    cart.Remove(item);
                    break;
                }
            }
            Session["cart"] = cart;
            TempData.Keep();
            return Redirect("checkout");
        }


        [AllowAnonymous]
        public ActionResult ContactUS()
        {
            return View();
        }
        
    }
}