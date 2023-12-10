using Microsoft.Data.SqlClient;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

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


        public void updatePhone(int id, Phone phone)
        {
            string sql = @"update Phone set PhoneName = @PhoneName, Manufacturer = @Manufacturer, Description = @Description, 
                            BoughtPrice = @BoughtPrice, Stock = @Stock, SoldPrice = @SoldPrice";
            if (phone.Avatar != null)
            {
                sql += ", Avatar = @Avatar where ID = @ID";
            }
            else
            {
                sql += " where ID = @ID";
            }
            SqlCommand sqlCommand = new SqlCommand(sql, DB.Instance.Connection);
            sqlCommand.Parameters.AddWithValue("@ID", id);
            sqlCommand.Parameters.AddWithValue("@PhoneName", phone.PhoneName);
            sqlCommand.Parameters.AddWithValue("@Manufacturer", phone.Manufacturer);
            sqlCommand.Parameters.AddWithValue("@BoughtPrice", phone.BoughtPrice);
            sqlCommand.Parameters.AddWithValue("@SoldPrice", phone.SoldPrice);
            sqlCommand.Parameters.AddWithValue("@Stock", phone.Stock);
            sqlCommand.Parameters.AddWithValue("@Description", phone.Description);

            if (phone.Avatar != null)
            {
                var encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(phone.Avatar));
                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    sqlCommand.Parameters.AddWithValue("@Avatar", stream.ToArray());
                }
            }

            try
            {
                sqlCommand.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine($"Updated {phone.ID} OK");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Updated {phone.ID} Fail: " + ex.Message);
            }
        }
    }
}
