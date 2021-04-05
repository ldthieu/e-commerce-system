using Microsoft.AspNetCore.Connections;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using E_Commerce_System.Models;

namespace E_Commerce_System.Services
{
    public class SqlService : ISqlService
    {
        private readonly ISqlServerSettings _settings;
        private static readonly object padlock = new object();
        public static SqlConnection dbConnection;
        public SqlService(ISqlServerSettings settings)
        {
            _settings = settings;
            lock (padlock)
            {
                if (dbConnection == null)
                {
                    CreateDbConnection(settings);
                }
            }
        }

        public SqlConnection CreateDbConnection(ISqlServerSettings settings)
        {
            string connectionString = "";
            connectionString = settings.ConnectionString;
            var conn = new SqlConnection(connectionString);
            conn.Open();
            return conn;
        }

        public DataTable GetDataTableWithStoredProcedure(string nameProc, params SqlParameter[] parameters)
        {
            DataTable table = GetDataTable(CommandType.StoredProcedure, nameProc, parameters);

            if (table != null)
                table.TableName = nameProc;

            return table;
        }

        public DataTable GetDataTable(CommandType commandType, string query, params SqlParameter[] parameters)
        {
            DataTable table = new DataTable();

            using (SqlConnection con = dbConnection)
            {
                con.Open();

                using (SqlDataAdapter adapter = new SqlDataAdapter(query, con))
                {
                    adapter.SelectCommand.CommandType = commandType;
                    adapter.SelectCommand.Parameters.AddRange(parameters);
                    adapter.SelectCommand.CommandTimeout = 180;
                    adapter.Fill(table);
                }

                con.Close();
            }

            return table;
        }
    }

    public interface ISqlService
    {
        SqlConnection CreateDbConnection(ISqlServerSettings settings);
        DataTable GetDataTableWithStoredProcedure(string nameProc, params SqlParameter[] parameters);
        DataTable GetDataTable(CommandType commandType, string query, params SqlParameter[] parameters);
    }
}
