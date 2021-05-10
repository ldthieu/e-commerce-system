using E_Commerce_System.Models;
using E_Commerce_System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace E_Commerce_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, IUserService userService)
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
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult LoginUser(UserInfo loginData)
        {
            var user = _userService.Authenticate(loginData.Email, loginData.Password);

            if (user != null)
            {
                var token = _userService.GenerateJSONWebToken(user);
                HttpContext.Session.SetString("JWToken", token.ToString());
            }
            return Redirect("~/Home/Index");
        }
    }
}
