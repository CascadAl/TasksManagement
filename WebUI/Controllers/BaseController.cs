using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public abstract class BaseController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            HandleErrorInfo errorInfo = new HandleErrorInfo(filterContext.Exception, filterContext.RouteData.Values["controller"].ToString(), filterContext.RouteData.Values["action"].ToString());
            //Log Exception e
            filterContext.ExceptionHandled = true;
            

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = 500;
                filterContext.Result = new JsonResult()
                {
                    Data = new { success = false, error = filterContext.Exception.ToString() },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                };
            }
            else
            {
                filterContext.Result = new ViewResult()
                {
                    ViewName = "Error",
                    ViewData = new ViewDataDictionary(errorInfo)
                    
                };
            }
        }
    }
}