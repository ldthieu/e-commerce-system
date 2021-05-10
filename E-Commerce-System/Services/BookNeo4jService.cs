using E_Commerce_System.Models;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_System.Services
{
    public class BookNeo4jService : IBookNeo4jService
    {
        private readonly INeo4jService _neo4JService;

        public BookNeo4jService()
        {

        }

        public BookNeo4jService(INeo4jService neo4JService)
        {
            _neo4JService = neo4JService;
        }
        public async Task<List<string>> Get()
        {
            IResultCursor cursor;
            var list = new List<String>();
            var session = _neo4JService.CreateSession();
            try
            {
                cursor = await session.RunAsync(@"MATCH (m:Movie) RETURN m.title AS title LIMIT 10");
                list = await cursor.ToListAsync(record =>
                record["title"].As<string>());
                return list.ToList();
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task<ReturnObject> AddCategory(string Name, string Code)
        {
            ReturnObject res = new ReturnObject();
            var statementText = new StringBuilder();
            statementText.Append("CREATE (category:Category {Name: $Name, Code: $Code})");
            var statementParameters = new Dictionary<string, object>
            {
                {"Name", Name },
                {"Code", Code },
            };
            var session = _neo4JService.CreateSession();
            try
            {
                var result = await session.WriteTransactionAsync(tx => tx.RunAsync(statementText.ToString(), statementParameters));
                res.statusCode = 200;
                res.message = "Tạo thành công";
            }
            catch(Exception ex)
            {
                res.statusCode = 500;
                res.message = ex.Message.ToString();
            }
            return res;
        }

    }
    public interface IBookNeo4jService
    {
        Task<List<string>> Get();
        Task<ReturnObject> AddCategory(string Name, string Code);
    }
}
