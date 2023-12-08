using MyShop.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
