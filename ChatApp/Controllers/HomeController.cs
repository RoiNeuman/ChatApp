using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ChatApp.Models;

namespace ChatApp.Controllers
{
    public class HomeController : Controller
    {
        public static IList<UserModel> Users = new List<UserModel>();

        // GET /home/AddUser?Name="UserName"
        public ActionResult AddUser(string name)
        {
            if (name == null || Users.Any(user => user.Name.Equals(name)))
                return Json(new {success = false}, JsonRequestBehavior.AllowGet);
            Console.WriteLine(name);
            UserModel newUser = new UserModel(name);
            Users.Add(newUser);
            Session["userName"] = name;
            ViewBag.userName = name;
            return Json(new { success = true, name = name }, JsonRequestBehavior.AllowGet);
        }

        // GET: Home
        public ActionResult Index(string user, string userKey)
        {
            foreach(string key in Session.Keys)
            {
                if (key.Equals("userName") && !Session["userName"].Equals("Session Time-out!"))
                    return Redirect("/chat");
            }
            return View();
        }
    }
}