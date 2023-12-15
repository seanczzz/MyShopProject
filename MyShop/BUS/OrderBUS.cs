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
    public class OrderBUS
    {
        private static OrderBUS? _instance = null;

        public static OrderBUS Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new OrderBUS();
                }

                return _instance;
            }
        }
        public int getOrderCountByWeek()
        {
            return OrderDAO.Instance.getOrderCountByWeek();
        }

        public int getOrderCountByMonth()
        {
            return OrderDAO.Instance.getOrderCountByMonth();
        }

        public BindingList<Order> GetAllOrdersByDate(DateTime FromDate, DateTime ToDate)
        {
            return OrderDAO.Instance.GetAllOrdersByDate(FromDate, ToDate);
        }

        public void AddOrderDetail(OrderDetails orderDetails)
        {
            OrderDAO.Instance.AddOrderDetail(orderDetails);
        }

        public void AddOrder(Order order)
        {
            OrderDAO.Instance.AddOrder(order);
        }

        public void DeleteOrderDetail(OrderDetails detail)
        {
            OrderDAO.Instance.DeleteOrderDetail(detail);
        }

        public void UpdateOrderDetail(int oldPhoneID, OrderDetails detail)
        {
            if (detail.Quantity >= 0)
            {
                OrderDAO.Instance.UpdateOrderDetail(oldPhoneID, detail);
            }
            else
            {
                throw new Exception("Invalid Quantity");
            }
        }

        public void UpdateOrder(Order order)
        {
            OrderDAO.Instance.UpdateOrder(order);
        }

        public void DeleteOrder(int id)
        {
            if (id > -1)
            {
                OrderDAO.Instance.DeleteOrder(id);
            }
        }
    }
}
