using System.Collections.Generic;
using System.Web.Mvc;
using ChatApp.Models;

namespace ChatApp.Controllers
{
    public class HomeController : Controller
    {
        public static IList<UserModel> Users = new List<UserModel>();
        public static IDictionary<string, UserModel> UserDictionary = new Dictionary<string, UserModel>();

        // GET /home/AddUser?Name="UserName"
        public ActionResult AddUser(string name)
        {
            if (name == null)
            {
                return Json(new {success = false}, JsonRequestBehavior.AllowGet);
            }
            UserModel newUser;
            if (UserDictionary.ContainsKey(name))
            {
                newUser = UserDictionary[name];
            }
            else
            {
                newUser = new UserModel(name);
                UserDictionary.Add(name, newUser);
            }
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