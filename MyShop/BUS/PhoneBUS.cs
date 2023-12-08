using MyShop.DAO;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.BUS
{
    internal class PhoneBUS
    {
        private PhoneDAO phoneDAO;

        public PhoneBUS()
        {
            phoneDAO = new PhoneDAO();

        }

        public int GetTotalPhone()
        {
            return phoneDAO.getTotalPhone();
        }

        public List<Phone> getTopFiveOutOfStock()
        {
            return phoneDAO.getTopFiveOutOfStock();
        }
    }

}
