using E_Commerce_System.Models;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
    public interface IBookNeo4jService
    {
        Task<List<string>> Get();
    }
}
