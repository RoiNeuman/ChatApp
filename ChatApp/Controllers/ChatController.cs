using System.Collections.Generic;
using System.Web.Mvc;
using ChatApp.Models;

namespace ChatApp.Controllers
{
    public class ChatController : Controller
    {
        public static IList<MessageModel> MessagesList = new List<MessageModel>();

        // GET /chat/statistics
        public JsonResult Statistics()
        {
            return Json(new StatisticsModel(), JsonRequestBehavior.AllowGet);
        }

        // GET: /chat/chat
        public ActionResult Chat()
        {
            var model = MessagesList;
            return View("Chat", model);
        }

        // GET: /chat
        public ActionResult Index()
        {
            return RedirectToAction("Chat");
        }
    }
}