using Projekt.Data;
using Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Projekt.Controllers
{
    public class HomeController : Controller
    {
        private ProjektContext db = new ProjektContext();

        //GET: Home
        public ActionResult Index()
        {
            if(Session["idUser"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        
        //GET: Register
        public ActionResult Register()
        {
            return View();
        }

        //Post: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                var check = db.Users.FirstOrDefault(s => s.Username == user.Username);
                if(check != null)
                {
                    ViewBag.error = "This username is already taken!";
                    return View();
                }
                else
                {
                    var check2 = db.Users.FirstOrDefault(s => s.Email == user.Email);
                    if(check2 != null)
                    {
                        ViewBag.error = "This Email is already taken!";
                        return View();
                    }
                    else
                    {
                        if (user.Username.Equals("Admin"))
                        {
                            user.Admin = true;
                        }
                        user.Password = GetMD5(user.Password);
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.Users.Add(user);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            if (ModelState.IsValid)
            {
                var f_password = GetMD5(password);
                var data = db.Users.FirstOrDefault(s => s.Username.Equals(username) && s.Password.Equals(f_password));
                if(data != null)
                {
                    //add session
                    Session["Username"] = data.Username;
                    Session["idUser"] = data.Id;
                    Session["Admin"] = data.Admin;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return View();
                }
            }
            return View();
        }

        //Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        private string GetMD5(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(password);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for(int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }
            return byte2String;
        }
    }
}