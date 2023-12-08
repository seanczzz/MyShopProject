using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DAO
{
    internal class OrderDAO
    {
        public int getOrderCountByWeek()
        {
            string sql = "select count(*) as week from Orders where datediff(day, OrderDate, GETDATE()) < 7";
            var command = new SqlCommand(sql, DB.Instance.Connection);

            var reader = command.ExecuteReader();

            int result = 0;

            if (reader.Read())
            {
                result = (int)reader["week"];
            }
            reader.Close();
            return result;
        }
        public int getOrderCountByMonth()
        {
            string sql = "select count(*) as month from Orders where datediff(day, OrderDate, GETDATE()) < 30";
            var command = new SqlCommand(sql, DB.Instance.Connection);

            var reader = command.ExecuteReader();

            int result = 0;

            if (reader.Read())
            {
                result = (int)reader["month"];
            }
            reader.Close();
            return result;
        }
    }
}
