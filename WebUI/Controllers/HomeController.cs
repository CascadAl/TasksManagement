using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{

    public class HomeController : BaseController
	{
		public ActionResult Index()
		{
            if(Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Group");
            }
            else
            {
                return View();
            }	
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Task Management contact page.";

			return View();
		}
	}
}