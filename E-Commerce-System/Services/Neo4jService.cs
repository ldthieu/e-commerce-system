using E_Commerce_System.Models;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace E_Commerce_System.Services
{
    public class Neo4jService : INeo4jService
    {
        private readonly INeo4jSettings _settings;
        public static IDriver Neo4jDriver;
        private static readonly object padlock = new object();
        public Neo4jService(INeo4jSettings settings)
        {
            _settings = settings;
            lock (padlock)
            {
                if (Neo4jDriver == null)
                {
                    CreateNeo4jDriver(settings);
                }
            }
        }

        public void CreateNeo4jDriver(INeo4jSettings settings)
        {
            try
            {
                Neo4jDriver = GraphDatabase.Driver(settings.ConnectionString, AuthTokens.Basic(settings.UserName, settings.Password));
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public IAsyncSession CreateSession()
        {
            return Neo4jDriver.AsyncSession();
        }

    }

    public interface INeo4jService
    {
        IAsyncSession CreateSession();
    }
}
