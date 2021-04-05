using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce_System.Services
{
    public class BookSqlService : IBookSqlService
    {
        private readonly ISqlService _sqlService;

        public BookSqlService()
        {

        }

        public BookSqlService(ISqlService sqlService)
        {
            _sqlService = sqlService;
        }

        public DataTable Get()
        {
            try
            {
                return  _sqlService.GetDataTableWithStoredProcedure("");
            }
            catch(Exception ex)
            {
                return _sqlService.GetDataTableWithStoredProcedure("");
            }
        }
    }
    public interface IBookSqlService
    {

    }
}
