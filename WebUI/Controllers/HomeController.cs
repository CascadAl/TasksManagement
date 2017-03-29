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
		private readonly ITestService _testService = null;

		public HomeController(ITestService testService)
		{
			_testService = testService;
		}

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		[Authorize]
		public ActionResult Test()
		{
			return View(_testService.GetAllMessages());
		}

		[Authorize]
		public ActionResult ExceptionTest()
		{
			int a = 5 - 5;
			a /= a;
			return View();
		}

	}
}