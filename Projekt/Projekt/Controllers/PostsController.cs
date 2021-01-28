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
using Projekt.ViewModels;

namespace Projekt.Controllers
{
    public class PostsController : Controller
    {
        private ProjektContext db = new ProjektContext();

        // GET: Posts
        public ActionResult Index()
        {
            if(Session["idUser"] == null)
            {
                return Redirect("/Home/Login");
            }
            List<PostViewModel> postViewModels = new List<PostViewModel>();
            foreach(Post post in db.Posts.ToList())
            {
                List<CommentViewModel> comments = new List<CommentViewModel>();
                foreach (Comment comment in db.Comments.Where(s => s.PostId == post.Id))
                {
                    var userC = db.Users.FirstOrDefault(s => s.Id == comment.UserId);
                    CommentViewModel commentViewModel = new CommentViewModel(comment, userC);
                    comments.Add(commentViewModel);
                }
                var user = db.Users.FirstOrDefault(s => s.Id == post.UserId);
                PostViewModel postViewModel = new PostViewModel(post, user, comments);
                postViewModels.Add(postViewModel);
            }
            return View(postViewModels);
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["idUser"] == null)
            {
                return Redirect("/Home/Login");
            }
            if (!(bool)Session["Admin"])
            {
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            if (Session["idUser"] == null)
            {
                return Redirect("/Home/Login");
            }
            return View();
        }

        // POST: Posts/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Post post)
        {
            if (Session["idUser"] == null)
            {
                return Redirect("/Home/Login");
            }
            var idUser = (int)Session["idUser"];
            var user = (User)db.Users.FirstOrDefault(s => s.Id.Equals(idUser));
            if (ModelState.IsValid)
            {
                post.User = user;
                post.UserId = user.Id;
                user.Posts.Add(post);
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["idUser"] == null)
            {
                return Redirect("/Home/Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            if (!(bool)Session["Admin"] && (int)Session["idUser"] != post.UserId)
            {
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Content")] Post post)
        {
            if (Session["idUser"] == null)
            {
                return Redirect("/Home/Login");
            }
            Post postToEdit = db.Posts.Find(post.Id);
            if(postToEdit == null)
            {
                return HttpNotFound();
            }
            if (!(bool)Session["Admin"] && (int)Session["idUser"] != postToEdit.UserId)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                postToEdit.Title = post.Title;
                postToEdit.Content = post.Content;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["idUser"] == null)
            {
                return Redirect("/Home/Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            if (!(bool)Session["Admin"] && (int)Session["idUser"] != post.UserId)
            {
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["idUser"] == null)
            {
                return Redirect("/Home/Login");
            }
            Post post = db.Posts.Find(id);
            if (!(bool)Session["Admin"] && (int)Session["idUser"] != post.UserId)
            {
                return RedirectToAction("Index");
            }
            foreach (Comment comment in db.Comments.Where(s => s.PostId == id))
            {
                db.Comments.Remove(comment);
            }
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment(PostViewModel item)
        {
            if (Session["idUser"] == null)
            {
                return Redirect("/Home/Login");
            }
            if (ModelState.IsValid)
            {
                PostViewModel postViewModel = item;
                Comment comment = postViewModel.Comment;
                comment.UserId = (int)Session["idUser"];
                comment.User = db.Users.Find(comment.UserId);
                comment.User.Comments.Add(comment);
                comment.PostId = postViewModel.Post.Id;
                comment.Post = postViewModel.Post;
                comment.Post.Comments.Add(comment);
                db.Comments.Add(comment);
                db.SaveChanges();
                
                CommentViewModel commentViewModel = new CommentViewModel(comment, comment.User);
                
                return PartialView("_PartialComment", commentViewModel);
            }
            return Redirect("/Posts/Index");
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
