using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.Data.SqlClient;
using MyShop.BUS;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Order = MyShop.DTO.Order;

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

        BindingList<OrderDetails> GetOrderDetails(int orderId)
        {
            string sql = "SELECT * FROM DetailOrder,Phone WHERE OrderID = @orderID and PhoneID = ID";

            var command = new SqlCommand(sql, DB.Instance.Connection);
            command.Parameters.AddWithValue("@orderID", orderId);

            var reader = command.ExecuteReader();

            var result = new BindingList<OrderDetails>();

            var phoneBUS = new PhoneBUS();
            while (reader.Read())
            {
                var OrderID = reader.GetInt32("OrderID");
                //var PhoneID = reader.GetInt32("PhoneID");
                var Quantity = reader.GetInt32("Quantity");

                int ID = (int)reader["ID"];
                string PhoneName = (String)reader["PhoneName"];
                string Manufacturer = (String)reader["Manufacturer"];

                int SoldPrice = (int)(decimal)reader["SoldPrice"];
                int Stock = (int)reader["Stock"];

                Phone phone = new Phone()
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

                OrderDetails order = new OrderDetails()
                {
                    OrderID = OrderID,
                    Phone = phone,
                    Quantity = Quantity
                };

                result.Add(order);
            }

            reader.Close();
            return result;
        }

        Order RDbToObject(SqlDataReader reader)
        {
            int ID = (int)reader["ID"];
            string CustomerName = (String)reader["CustomerName"];
            var OrderDate = DateOnly.Parse(DateTime.Parse(reader["OrderDate"].ToString()).Date.ToShortDateString());
            var Status = (System.Int16)reader["Status"];
            string Address = (String)reader["Address"];          

            Order order = new Order()
            {
                ID = ID,
                CustomerName = CustomerName,
                OrderDate = OrderDate,
                Status = (Order.OrderStatusEnum)Status,
                Address = Address,
            };
            return order;
        }
        BindingList<Order> SelectFromOrderQuery(SqlCommand command)
        {
            var reader = command.ExecuteReader();

            var result = new BindingList<Order>();

            while (reader.Read())
            {
                result.Add(RDbToObject(reader));
            }
            reader.Close();

            foreach (var order in result)
            {
                BindingList<OrderDetails> OrderDetailsList = GetOrderDetails(order.ID);
                order.OrderDetailsList = OrderDetailsList;
            }

            return result;
        }

        public BindingList<Order> GetAllOrdersByDate(DateTime FromDate, DateTime ToDate)
        {
            string sql = "SELECT * FROM Orders where OrderDate >= @fromDate and OrderDate <= @toDate";

            var command = new SqlCommand(sql, DB.Instance.Connection);
            command.Parameters.AddWithValue("@fromDate", FromDate);
            command.Parameters.AddWithValue("@toDate", ToDate);

            return SelectFromOrderQuery(command);
        }

        public void AddOrderDetail(OrderDetails orderDetails)
        {
            string sql = @"INSERT INTO DetailOrder(OrderID, PhoneID, Quantity) 
                            VALUES (@OrderID, @PhoneID, @Quantity)";
            SqlCommand sqlCommand = new SqlCommand(sql, DB.Instance.Connection);

            sqlCommand.Parameters.AddWithValue("@OrderID", orderDetails.OrderID);
            sqlCommand.Parameters.AddWithValue("@PhoneID", orderDetails.Phone.ID);
            sqlCommand.Parameters.AddWithValue("@Quantity", orderDetails.Quantity);

            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void DeleteOrderDetail(OrderDetails detail)
        {
            string sql = @"DELETE FROM DetailOrder WHERE OrderID=@OrderID and PhoneID=@PhoneID and Quantity = @Quantity";
            SqlCommand command = new SqlCommand(sql, DB.Instance.Connection);
            command.Parameters.AddWithValue("@OrderID", detail.OrderID);
            command.Parameters.AddWithValue("@PhoneID", detail.Phone.ID);
            command.Parameters.AddWithValue("@Quantity", detail.Quantity);

            try
            {
                command.ExecuteNonQuery();
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void UpdateOrderDetail(int oldPhoneID, OrderDetails detail)
        {
            string sql = "UPDATE DetailOrder SET Quantity = @Quantity, PhoneID = @PhoneID WHERE OrderID = @OrderID AND PhoneID = @oldPhoneID";
            SqlCommand sqlCommand = new SqlCommand(sql, DB.Instance.Connection);

            sqlCommand.Parameters.AddWithValue("@OrderID", detail.OrderID);
            sqlCommand.Parameters.AddWithValue("@PhoneID", detail.Phone.ID);
            sqlCommand.Parameters.AddWithValue("@oldPhoneID", oldPhoneID);
            sqlCommand.Parameters.AddWithValue("@Quantity", detail.Quantity);

            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void UpdateOrder(Order order)
        {
            string sql = @"update Orders 
                        SET CustomerName = @CustomerName, OrderDate = @OrderDate, Status =  @Status, Address = @Address 
                            where ID = @ID";
            SqlCommand sqlCommand = new SqlCommand(sql, DB.Instance.Connection);

            sqlCommand.Parameters.AddWithValue("@ID", order.ID);
            sqlCommand.Parameters.AddWithValue("@CustomerName", order.CustomerName);
            sqlCommand.Parameters.AddWithValue("@OrderDate", DateTime.Parse(order.OrderDate.ToString()));
            sqlCommand.Parameters.AddWithValue("@Status", order.Status);
            sqlCommand.Parameters.AddWithValue("@Address", order.Address);


            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void AddOrder(Order order)
        {
            var sql = @"INSERT INTO Orders(CustomerName, OrderDate, Status, Address) 
                        values (@CustomerName, @OrderDate, @Status, @Address);
                        select ident_current('Orders');"; // Them VoucherID sau
            SqlCommand sqlCommand = new SqlCommand(sql, DB.Instance.Connection);

            sqlCommand.Parameters.AddWithValue("@CustomerName", order.CustomerName);
            sqlCommand.Parameters.AddWithValue("@OrderDate", DateTime.Parse(order.OrderDate.ToString()));
            sqlCommand.Parameters.AddWithValue("@Status", order.Status);
            sqlCommand.Parameters.AddWithValue("@Address", order.Address);

            try
            {
                int id = (int)((decimal)sqlCommand.ExecuteScalar());
                order.ID = id;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void DeleteOrder(int id)
        {
            string sql = "delete from Orders where ID = @ID";
            SqlCommand sqlCommand = new SqlCommand(sql, DB.Instance.Connection);

            sqlCommand.Parameters.AddWithValue("@ID", id);

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
