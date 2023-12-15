using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using MyShop.DAO;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Order = MyShop.DTO.Order;

namespace MyShop.BUS
{
    internal class OrderBUS
    {
        private OrderDAO orderDAO;

        public OrderBUS()
        {
            orderDAO = new OrderDAO();
        }
        public int getOrderCountByWeek()
        {
            return orderDAO.getOrderCountByWeek();
        }

        public int getOrderCountByMonth()
        {
            return orderDAO.getOrderCountByMonth();
        }

        public BindingList<Order> GetAllOrdersByDate(DateTime FromDate, DateTime ToDate)
        {
            return orderDAO.GetAllOrdersByDate(FromDate, ToDate);
        }

        public void AddOrderDetail(OrderDetails orderDetails)
        {
            orderDAO.AddOrderDetail(orderDetails);
        }

        public void AddOrder(Order order)
        {
            orderDAO.AddOrder(order);
        }

        public void DeleteOrderDetail(OrderDetails detail)
        {
            orderDAO.DeleteOrderDetail(detail);
        }

        public void UpdateOrderDetail(int oldPhoneID, OrderDetails detail)
        {
            if (detail.Quantity >= 0)
            {
                orderDAO.UpdateOrderDetail(oldPhoneID, detail);
            }
            else
            {
                throw new Exception("Invalid Quantity");
            }
        }

        public void UpdateOrder(Order order)
        {
            orderDAO.UpdateOrder(order);
        }

        public void DeleteOrder(int id)
        {
            if (id > -1)
            {
                orderDAO.DeleteOrder(id);
            }
        }
    }
}
