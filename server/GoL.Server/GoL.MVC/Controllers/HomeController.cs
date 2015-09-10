using GoL.MVC.App_Infrastructure;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoL.MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [System.Web.Mvc.Authorize]
        public ActionResult Game()
        {
            return View();
        }

        public ActionResult Observe(string username)
        {
            //TODO: if username has a stream running

            //connectionId only available via signalr connection
            //GlobalHost.ConnectionManager.GetHubContext<GameHub>().Groups.Add(connectionId, username);

            ViewBag.Username = username;

            return View();
        }
    }
}
