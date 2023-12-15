using Microsoft.Data.SqlClient;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MyShop.DAO
{
    public class PhoneDAO
    {
        private static PhoneDAO? _instance = null;

        public static PhoneDAO Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PhoneDAO();
                }

                return _instance;
            }
        }
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

        public Phone getPhoneByID(int id)
        {
            string sql = "select * from Phone WHERE ID = @phoneID";
            var command = new SqlCommand(sql, DB.Instance.Connection);

            command.Parameters.AddWithValue("@phoneID", id);

            var reader = command.ExecuteReader();

            Phone? phone = null;
            if (reader.Read())
            {
                int ID = (int)reader["ID"];
                string PhoneName = (String)reader["PhoneName"];
                string Manufacturer = (String)reader["Manufacturer"];

                int SoldPrice = (int)(decimal)reader["SoldPrice"];
                int Stock = (int)reader["Stock"];

                phone = new Phone()
                {
                    ID = ID,
                    PhoneName = PhoneName,
                    Manufacturer = Manufacturer,
                    SoldPrice = SoldPrice,
                    Stock = Stock,
                };

                byte[] byteAvatar = new byte[5];
                if (reader["Avatar"] != System.DBNull.Value)
                {

                    byteAvatar = (byte[])reader["Avatar"];

                    using (MemoryStream ms = new MemoryStream(byteAvatar))
                    {
                        var image = new BitmapImage();
                        image.BeginInit();
                        image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.UriSource = null;
                        image.StreamSource = ms;
                        image.EndInit();
                        image.Freeze();
                        phone.Avatar = image;
                    }
                }
            }
            reader.Close();
            return phone;
        }
        public List<Phone> getAllPhones()
        {
            List<Phone> result = new List<Phone>();
            
            string sql = "select ID,PhoneName,Manufacturer,Stock,SoldPrice,BoughtPrice,Description,CatID,Avatar from Phone";
            var command = new SqlCommand(sql, DB.Instance.Connection);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = (int)reader["ID"];
                    string phoneName = (string)reader["PhoneName"];
                    string manufacturer = (string)reader["Manufacturer"];
                    int stock = (int)reader["Stock"];
                    int soldPrice = (int)(decimal)reader["SoldPrice"];
                    int boughtPrice = (int)(decimal)reader["BoughtPrice"];
                    string description = (String)reader["Description"];
                    int catId = (int)reader["CatID"];

                    Phone Phone = new Phone()
                    {
                        ID = id,
                        PhoneName = phoneName,
                        Manufacturer = manufacturer,
                        Stock = stock,
                        SoldPrice = soldPrice,
                        BoughtPrice = boughtPrice,
                        Description = description,
                        CatID = catId,
                    };

                    if (!reader["Avatar"].Equals(DBNull.Value))
                    {
                        var byteAvatar = (byte[])reader["Avatar"];
                        using (MemoryStream ms = new MemoryStream(byteAvatar))
                        {
                            var image = new BitmapImage();
                            image.BeginInit();
                            image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                            image.CacheOption = BitmapCacheOption.OnLoad;
                            image.UriSource = null;
                            image.StreamSource = ms;
                            image.EndInit();
                            image.Freeze();
                            Phone.Avatar = image;
                        }
                    }
                    result.Add(Phone);
                }
                reader.Close();
            }

            return result;

        }

        public void InsertNewPhone(Phone phone)
        {
            string sql;
            if (phone.Avatar != null)
            {
                sql = @"INSERT INTO Phone (PhoneName, Manufacturer, Stock, Description, BoughtPrice,
                        SoldPrice, CatID, UploadDate, Avatar) VALUES (@PhoneName, @Manufacturer, @Stock, 
                        @Description, @BoughtPrice, @SoldPrice, @CatID, @UploadDate, @Avatar);";
            }
            else
            {
                sql = @"INSERT INTO Phone (PhoneName, Manufacturer, Stock, Description, BoughtPrice,
                        SoldPrice, CatID, UploadDate) VALUES (@PhoneName, @Manufacturer, @Stock, 
                        @Description, @BoughtPrice, @SoldPrice, @CatID, @UploadDate);";
            }
            sql += "select ident_current('Phone');";
            SqlCommand command = new SqlCommand(sql, DB.Instance.Connection);

            command.Parameters.AddWithValue("@PhoneName", phone.PhoneName);
            command.Parameters.AddWithValue("@Manufacturer", phone.Manufacturer);
            command.Parameters.AddWithValue("@Stock", phone.Stock);
            command.Parameters.AddWithValue("@Description", phone.Description);
            command.Parameters.AddWithValue("@BoughtPrice", phone.BoughtPrice);
            command.Parameters.AddWithValue("@SoldPrice", phone.SoldPrice);
            command.Parameters.AddWithValue("@UploadDate", phone.UploadDate);
            command.Parameters.AddWithValue("@CatID", phone.CatID);

            if (phone.Avatar != null)
            {
                var encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(phone.Avatar));
                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    command.Parameters.AddWithValue("@Avatar", stream.ToArray());
                }
            }

            try
            {
                int id = (int)((decimal)command.ExecuteScalar());
                phone.ID = id;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void DeletePhone(int phoneID)
        {
            var sql = "DELETE FROM Phone WHERE ID = @ID";
            SqlCommand sqlCommand = new SqlCommand(sql, DB.Instance.Connection);
            sqlCommand.Parameters.AddWithValue("@ID", phoneID);

            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
