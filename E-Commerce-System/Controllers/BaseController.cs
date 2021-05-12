using E_Commerce_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce_System.Controllers
{
    public class BaseController : Controller
    {
        //protected readonly ADBMSContext aDBMSContext;
        //public BaseController(ADBMSContext aDBMSContext)
        //{
        //    this.aDBMSContext = aDBMSContext;
        //}
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!User.Identity.IsAuthenticated)
            {
                HttpContext.Session.Remove("JWToken");
                //HttpContext.Response.Redirect("/Home/Index");
                //return;
                context.Result = RedirectToAction("Index", "Home");
                return;
            }
            else
            {
                base.OnActionExecuting(context);
            }
        }
    }
}
