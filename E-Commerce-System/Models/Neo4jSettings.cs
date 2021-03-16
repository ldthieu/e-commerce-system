using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce_System.Models
{
    public class Neo4jSettings : INeo4jSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public interface INeo4jSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
