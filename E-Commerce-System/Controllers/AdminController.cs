using E_Commerce_System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace E_Commerce_System.Controllers
{
    public class AdminController : BaseController
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IUserService _userService;

        public AdminController(ILogger<AdminController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        //[Authorize]
        public IActionResult Index()
        {
            //var user = _userService.Authenticate("admin@gmail.com", "abc-12345");
            //if (user != null)
            //{
            //    var token = _userService.GenerateJSONWebToken(user);
            //    HttpContext.Session.SetString("JWToken", new JwtSecurityTokenHandler().WriteToken(token));
            //    var identity = new System.Security.Principal.GenericIdentity(user.UserName);
            //    var principal = new GenericPrincipal(identity, new string[0]);
            //    HttpContext.User = principal;
            //    Thread.CurrentPrincipal = principal;
            //}

            //var JWToken = HttpContext.Session.GetString("JWToken");
            //if (string.IsNullOrEmpty(JWToken))
            //{
            //    return Redirect("~/Home/Index");
            //}
            
            return View();
        }

    }
}
