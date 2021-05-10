using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_Commerce_System.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Text;
using E_Commerce_System.Services;
using E_Commerce_System.Common;

namespace E_Commerce_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        //private readonly ADBMSContext _context;
        private readonly IUserService _userService;

        public TokenController(IConfiguration config, IUserService userService)
        {
            _configuration = config;
            _userService = userService;
        }

        [HttpPost]
        [Route("/Create")]
        public async Task<IActionResult> Create(UserInfo _userData)
        {

            if (_userData != null && _userData.Email != null && _userData.Password != null)
            {
                if (_userService.CheckUser(_userData))
                {
                    _userService.Create(_userData);
                    return Ok();
                }
                else
                {
                    return BadRequest("UserName hoặc Email đã tồn tại");
                }
            }
            else
            {
                return BadRequest("Thông tin đăng ký không hợp lệ");
            }
        }

        [HttpPost]
        [Route("/Authenticate")]
        public async Task<IActionResult> Authenticate(UserInfo _userData)
        {

            if (_userData != null && _userData.Email != null && _userData.Password != null)
            {
                var user = _userService.Authenticate(_userData.Email, _userData.Password);

                if (user != null)
                {
                    var token = _userService.GenerateJSONWebToken(user);
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

    }
}