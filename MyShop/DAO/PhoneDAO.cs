using Microsoft.Data.SqlClient;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DAO
{
    internal class PhoneDAO
    {
        public int getTotalPhone()
        {
            var sql = "select count(*) as total_phone from Phone";
            var command = new SqlCommand(sql, DB.Instance.Connection);
            var reader = command.ExecuteReader();

            int result = 0;
            if (reader.Read())
            {
                result = (int)reader["total_phone"];
            }
            reader.Close();
            return result;
        }

        public List<Phone> getTopFiveOutOfStock()
        {
            var sql = @"select Top 5 ID,PhoneName,Manufacturer,Stock,SoldPrice 
                        from Phone 
                        where stock < 5
                        order by Stock";
            var command = new SqlCommand(sql, DB.Instance.Connection);
            
            List<Phone> result = new List<Phone>();
            
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var id = (int)reader["ID"];
                    var phoneName = (string)reader["PhoneName"];
                    var manufacturer = (string)reader["Manufacturer"];
                    var stock = (int)reader["Stock"];
                    var soldPrice = (int)(decimal)reader["SoldPrice"];

                    var Phone = new Phone()
                    {
                        ID = id,
                        PhoneName = phoneName,
                        Manufacturer = manufacturer,
                        Stock = stock,
                        SoldPrice = soldPrice
                    };
                    result.Add(Phone);
                }
                reader.Close();
            }

            return result;
        }
    }
}
