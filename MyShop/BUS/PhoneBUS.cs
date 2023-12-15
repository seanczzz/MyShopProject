using MyShop.DAO;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public Phone getPhoneByID(int id)
        {
            return phoneDAO.getPhoneByID(id);
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

        public List<Phone> getAllPhones()
        {
            return phoneDAO.getAllPhones();
        }

        public void AddPhone(Phone phone)
        {
            phone.UploadDate = DateTime.Now.Date;
            phoneDAO.InsertNewPhone(phone);
        }

        public void DeletePhone(int phoneID)
        {
            phoneDAO.DeletePhone(phoneID);
        }
    }

}
