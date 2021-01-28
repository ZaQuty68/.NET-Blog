using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Projekt.Data;
using Projekt.Models;

namespace Projekt.Controllers
{
    public class CommentsController : Controller
    {
        private ProjektContext db = new ProjektContext();

        // GET: Comments
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
            return View(db.Comments.ToList());
        }

        // GET: Comments/Details/5
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
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
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

        // POST: Comments/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Content")] Comment comment)
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
                comment.UserId = (int)Session["idUser"];
                comment.User = db.Users.Find(comment.UserId);
                comment.User.Comments.Add(comment);
                comment.PostId = 1;
                comment.Post = db.Posts.Find(1);
                comment.Post.Comments.Add(comment);
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(comment);
        }

        // GET: Comments/Edit/5
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
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Content")] Comment comment)
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
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(comment);
        }

        // GET: Comments/Delete/5
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
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
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
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
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
