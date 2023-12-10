using MyShop.DAO;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public void updatePhone(int ID, Phone phone)
        {
            //Debug.WriteLine(phone.Stock);
            if (phone.Stock < 0)
            {
                throw new Exception("Invalid stock");
            }
            else if (phone.BoughtPrice < 0 || phone.SoldPrice < 0)
            {
                throw new Exception("Invalid price");
            }
            else
            {
                phoneDAO.updatePhone(ID, phone);
            }
        }
    }

}
