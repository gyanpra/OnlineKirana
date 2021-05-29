using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.IO;
using System.Web.Mvc;
using OnlineKirana;
using OnlineKirana.Models;

namespace OnlineKirana.Controllers
{
    public class ManageProductsController : Controller
    {
        public OnlineKiranaEntities db = new OnlineKiranaEntities();
        // GET: ManageProducts

        public ActionResult Login()
        {
            ViewBag.msg = "";
            return View();
        }

        [HttpPost]
        public ActionResult Login(Admin admn)
        {
            var a = db.Admins.Where(l => l.Email.Equals(admn.Email) && l.Password.Equals(admn.Password)).ToList();
            if (a.Count > 0)
            {
                Session["Email"] = admn.Email.ToString();
                return RedirectToAction("Index", "ManageProducts");

            }
            else
            {
                ViewBag.msg = "Invalid UserName or Password";
            }
            return View("Index");
        }
        public ActionResult Index()
        {
            if (Session["Email"] == null)
            {
                ViewBag.msg = "Log in as Admin to proceed.";
                return RedirectToAction("Login");
            }
            var data = db.Products.ToList();
            return View(data);
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid == true)
            {
                string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                string extension = Path.GetExtension(product.ImageFile.FileName);
                HttpPostedFileBase postedFile = product.ImageFile;
                int length = postedFile.ContentLength;

                if(extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                {
                    if(length<= 4194304)
                    {
                        fileName = fileName + extension;
                        product.ProductImage = "~/content/Images/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/Content/Images/"),fileName);
                        product.ImageFile.SaveAs(fileName);
                        db.Products.Add(product);
                        int a=db.SaveChanges();

                        if (a > 0)
                        {
                            TempData["CreateMessage"] = "<script>alert('Product Added Successfully.')</script>";
                            ModelState.Clear();
                            return RedirectToAction("Index","ManageProducts");
                        }
                        else
                        {
                            TempData["CreateMessage"] = "<script>alert('Product not Added.')</script>";
                        }

                    }
                    else
                    {
                        TempData["SizeMessage"] = "<script>alert('Image should be less than 4 MB.')</script>";
                    }

                }
                else
                {
                    TempData["ExtensionMessage"] = "<script>alert('format not supported')</script>";
                }
            }
            return View();

        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            var Productrow = db.Products.Where(model => model.ProductID == id).FirstOrDefault();
            Session["Image"] = Productrow.ProductImage;

            return View(Productrow);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid == true)
            {
                if (product.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                    string extension = Path.GetExtension(product.ImageFile.FileName);
                    HttpPostedFileBase postedFile = product.ImageFile;
                    int length = postedFile.ContentLength;

                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    {
                        if (length <= 4194304)
                        {
                            fileName = fileName + extension;
                            product.ProductImage = "~/content/Images/" + fileName;
                            fileName = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                            product.ImageFile.SaveAs(fileName);
                            db.Entry(product).State = EntityState.Modified;
                            int a = db.SaveChanges();

                            if (a > 0)
                            {
                                TempData["UpdateMessage"] = "<script>alert('Product Updated Successfully.')</script>";
                                ModelState.Clear();
                                return RedirectToAction("Index", "ManageProducts");
                            }
                            else
                            {
                                TempData["UpdateMessage"] = "<script>alert('Product not Updated.')</script>";
                            }

                        }
                        else
                        {
                            TempData["SizeMessage"] = "<script>alert('Image should be less than 4 MB.')</script>";
                        }

                    }
                    else
                    {
                        TempData["ExtensionMessage"] = "<script>alert('format not supported')</script>";
                    }
                }
                else
                {
                    product.ProductImage = Session["Image"].ToString();
                    db.Entry(product).State = EntityState.Modified;
                    int a = db.SaveChanges();

                    if (a > 0)
                    {
                        TempData["UpdateMessage"] = "<script>alert('Product Updated Successfully.')</script>";
                        ModelState.Clear();
                        return RedirectToAction("Index", "ManageProducts");
                    }
                    else
                    {
                        TempData["UpdateMessage"] = "<script>alert('Product not Updated.')</script>";
                    }
                }
            }

            return View();
        }



        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }


        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "ManageProducts");
        }


    }
}