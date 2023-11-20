using Dapper;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace FoodShop_SWP.Models.Common
{
    public class StatisticalAccess
    {
        private readonly IConfiguration _configuration;

        public StatisticalAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        
        public StatiscalViewModel Statiscal()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var connect = new SqlConnection(connectionString))
            {
                var item = connect.QueryFirstOrDefault<StatiscalViewModel>("sp_ThongKe", commandType: CommandType.StoredProcedure);
                return item;
            }
        }
    }
}
