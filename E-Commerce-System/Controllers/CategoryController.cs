using E_Commerce_System.Models;
using E_Commerce_System.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace E_Commerce_System.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : BaseController
    {
        private readonly ADBMSContext _aDBMSContext;
        private readonly ICategoryService _categoryService;
        public CategoryController(ADBMSContext aDBMSContext, ICategoryService categoryService)
        {
            _aDBMSContext = aDBMSContext;
            _categoryService = categoryService;
        }

        [Authorize]
        [HttpGet]
        public Task<List<Category>> Get()
        {
            try
            {
                var res = _categoryService.GetListCategory();
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("/GetListCategory")]
        public Task<List<Category>> GetListCategory()
        {
            try
            {
                var res = _categoryService.GetListCategory();
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
