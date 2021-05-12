using E_Commerce_System.Models;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce_System.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetListCategory();
    }
    public class CategoryService : ICategoryService
    {
        private readonly INeo4jService _neo4JService;

        public CategoryService()
        {

        }

        public CategoryService(INeo4jService neo4JService)
        {
            _neo4JService = neo4JService;
        }

        public async Task<List<Category>> GetListCategory()
        {
            List<Category> lsCategory = new List<Category>();
            IResultCursor cursor;
            var session = _neo4JService.CreateSession();
            try
            {
                cursor = await session.RunAsync(@"MATCH (cate:Category)
                                                OPTIONAL MATCH (parent)-->(cate)
                                                RETURN cate.ID, cate.Name, parent.ID, parent.Name");

                var list = await cursor.ToListAsync();
                lsCategory = list.Select(p => new Category
                {
                    CateName = p["cate.Name"].ToString(),
                    CateId = p["cate.ID"].ToString(),
                    ParentId = string.IsNullOrEmpty((string)p["parent.ID"]) ? "" : p["parent.ID"].ToString(),
                    ParentName = string.IsNullOrEmpty((string)p["parent.Name"]) ? "" : p["parent.Name"].ToString(),
                }).ToList();

                return lsCategory;
            }
            finally
            {
                await session.CloseAsync();
            }
        }

    }
}
