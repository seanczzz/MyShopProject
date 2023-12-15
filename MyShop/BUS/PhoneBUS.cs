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
    public class PhoneBUS
    {
        private static PhoneBUS? _instance = null;

        public static PhoneBUS Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PhoneBUS();
                }

                return _instance;
            }
        }
        public int GetTotalPhone()
        {
            return PhoneDAO.Instance.getTotalPhone();
        }

        public Phone getPhoneByID(int id)
        {
            return PhoneDAO.Instance.getPhoneByID(id);
        }
        public List<Phone> getTopFiveOutOfStock()
        {
            return PhoneDAO.Instance.getTopFiveOutOfStock();
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
                PhoneDAO.Instance.updatePhone(ID, phone);
            }
        }

        public List<Phone> getAllPhones()
        {
            return PhoneDAO.Instance.getAllPhones();
        }

        public void AddPhone(Phone phone)
        {
            phone.UploadDate = DateTime.Now.Date;
            PhoneDAO.Instance.InsertNewPhone(phone);
        }

        public void DeletePhone(int phoneID)
        {
            PhoneDAO.Instance.DeletePhone(phoneID);
        }

        public List<BestSellingPhone> getBestSellingPhonesInWeek(DateTime src)
        {
            return PhoneDAO.Instance.getBestSellingPhonesInWeek(src);
        }

        public List<BestSellingPhone> getBestSellingPhonesInMonth(DateTime src)
        {
            return PhoneDAO.Instance.getBestSellingPhonesInMonth(src);
        }

        public List<BestSellingPhone> getBestSellingPhonesInYear(DateTime src)
        {
            return PhoneDAO.Instance.getBestSellingPhonesInYear(src);
        }

        public List<Phone> getPhonesByCategory(int srcCategoryID)
        {
            return PhoneDAO.Instance.getPhonesByCategory(srcCategoryID);
        }
    }

}
