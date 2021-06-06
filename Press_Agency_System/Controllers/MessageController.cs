using Press_Agency_System.Models;
using Press_Agency_System.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Press_Agency_System.Controllers
{
    public class MessageController : Controller
    {
        // GET: Message
        [HttpGet]
        public ActionResult Messages()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetChatbox(string toUserId)
        {
            ChatBoxModel chatBoxModel = new AppServices().GetChatbox(toUserId);
            return PartialView("~/Views/Shared/_ChatBox.cshtml", chatBoxModel);
        }

        [HttpPost]
        public ActionResult SendMessage(string toUserId, string message)
        {
            return Json(new AppServices().SendMessage(toUserId, message));
        }
        
    }
}