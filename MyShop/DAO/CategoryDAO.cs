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
    public class CategoryDAO
    {
        private static CategoryDAO? _instance = null;

        public static CategoryDAO Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CategoryDAO();
                }

                return _instance;
            }
        }
        public BindingList<Category> getAllCategories()
        {
            BindingList<Category> result = new BindingList<Category>();
            var sql = "select * from Category";
            var command = new SqlCommand(sql, DB.Instance.Connection);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var id = (int)reader["ID"];
                    var categoryName = (string)reader["CatName"];

                    var category = new Category()
                    {
                        ID = id,
                        CatName = categoryName,                     
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
                            category.Avatar = image;
                        }
                    }

                    result.Add(category);
                }
                reader.Close();
            }
            return result;
        }

        public void InsertCategory(Category category)
        {

            string sql;
            if (category.Avatar != null)
            {
                sql = @"INSERT INTO Category (CatName, Avatar) VALUES 
                        (@CatName, @Avatar)";
            }
            else
            {
                sql = @"INSERT INTO Category (CatName) VALUES 
                        (@CatName)";
            }

            sql += "select ident_current('Category');";
            SqlCommand sqlCommand = new SqlCommand(sql, DB.Instance.Connection);

            sqlCommand.Parameters.AddWithValue("@CatName", category.CatName);

            if (category.Avatar != null)
            {
                var encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(category.Avatar));
                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    sqlCommand.Parameters.AddWithValue("@Avatar", stream.ToArray());
                }
            }

            try
            {
                int id = (int)((decimal)sqlCommand.ExecuteScalar());
                category.ID = id;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public void DeleteCategory(int id)
        {
            var sql = "DELETE FROM Category WHERE ID = @ID";
            SqlCommand sqlCommand = new SqlCommand( sql, DB.Instance.Connection);
            sqlCommand.Parameters.AddWithValue("@ID", id);
          
            try
            {
                sqlCommand.ExecuteNonQuery();
                sql = "delete from Phone where CatID = @ID";
                sqlCommand = new SqlCommand(sql, DB.Instance.Connection);
                sqlCommand.Parameters.AddWithValue("@ID", id);
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void updateCategory(Category category)
        {
            string sql;
            if (category.Avatar != null)
            {
                sql = "update Category set CatName = @CatName, Avatar = @Avatar where ID = @ID";
            }
            else
            {
                sql = "update Category set CatName = @CatName where ID = @ID";
            }
            SqlCommand sqlCommand = new SqlCommand(sql, DB.Instance.Connection);
            sqlCommand.Parameters.AddWithValue("@ID", category.ID);
            sqlCommand.Parameters.AddWithValue("@CatName", category.CatName);

            if (category.Avatar != null)
            {
                var encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(category.Avatar));
                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    sqlCommand.Parameters.AddWithValue("@Avatar", stream.ToArray());
                }
            }

            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }


            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public int isExistCategory(string categoryName)
        {
            string sql = "select ID from Category where CatName = @CatName";
            SqlCommand command = new SqlCommand(sql, DB.Instance.Connection);
            command.Parameters.AddWithValue("@CatName", categoryName);

            var reader = command.ExecuteReader();
            int ID = 0;
            while (reader.Read())
            {
                ID = (int)reader["ID"];
            }
            reader.Close();
            return ID;
        }
    }
}
