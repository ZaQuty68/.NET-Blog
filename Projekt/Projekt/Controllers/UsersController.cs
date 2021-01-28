using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Projekt.Data;
using Projekt.Models;

namespace Projekt.Controllers
{
    public class UsersController : Controller
    {
        private ProjektContext db = new ProjektContext();

        // GET: Users
        public ActionResult Index()
        {
            if (Session["idUser"] == null)
            {
                return Redirect("/Home/Login");
            }
            if (!(bool)Session["Admin"])
            {
                return Redirect("/Home/Index");
            }
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["idUser"] == null)
            {
                return Redirect("/Home/Login");
            }
            if (!(bool)Session["Admin"])
            {
                return Redirect("/Home/Index");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            if (Session["idUser"] == null)
            {
                return Redirect("/Home/Login");
            }
            if (!(bool)Session["Admin"])
            {
                return Redirect("/Home/Index");
            }
            return View();
        }

        // POST: Users/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Username,Password,ConfirmPassword,Email,PhoneNumber,Admin")] User user)
        {
            if (Session["idUser"] == null)
            {
                return Redirect("/Home/Login");
            }
            if (!(bool)Session["Admin"])
            {
                return Redirect("/Home/Index");
            }
            if (ModelState.IsValid)
            {
                user.Password = GetMD5(user.Password);
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["idUser"] == null)
            {
                return Redirect("/Home/Login");
            }
            if (!(bool)Session["Admin"])
            {
                return Redirect("/Home/Index");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Username,Password,ConfirmPassword,Email,PhoneNumber,Admin")] User user)
        {
            if (Session["idUser"] == null)
            {
                return Redirect("/Home/Login");
            }
            if (!(bool)Session["Admin"])
            {
                return Redirect("/Home/Index");
            }
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["idUser"] == null)
            {
                return Redirect("/Home/Login");
            }
            if (!(bool)Session["Admin"])
            {
                return Redirect("/Home/Index");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["idUser"] == null)
            {
                return Redirect("/Home/Login");
            }
            if (!(bool)Session["Admin"])
            {
                return Redirect("/Home/Index");
            }
            foreach(Post post in db.Posts.Where(s => s.UserId == id))
            {
                db.Posts.Remove(post);
            }
            foreach(Comment comment in db.Comments.Where(s => s.UserId == id))
            {
                db.Comments.Remove(comment);
            }
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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

        private string GetMD5(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(password);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }
            return byte2String;
        }
    }
}
