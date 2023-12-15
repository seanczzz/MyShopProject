using MyShop.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.BUS
{
    public class StatisticsBUS
    {
        private static StatisticsBUS? _instance = null;

        public static StatisticsBUS Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new StatisticsBUS();
                }

                return _instance;
            }
        }

        public string getTotalRevenueUntilDate(DateTime src)
        {
            return StatisticsDAO.Instance.getTotalRevenueUntilDate(src);
        }

        public string getTotalProfitUntilDate(DateTime src)
        {
            return StatisticsDAO.Instance.getTotalProfitUntilDate(src);
        }

        public int getTotalOrdersUntilDate(DateTime src)
        {
            return StatisticsDAO.Instance.getTotalOrdersUntilDate(src);
        }

        public List<Tuple<string, decimal>> getDailyRevenue(DateTime src)
        {
            return StatisticsDAO.Instance.getDailyRevenue(src);
        }

        public List<Tuple<string, decimal>> getWeeklyRevenue(DateTime src)
        {
            return StatisticsDAO.Instance.getWeeklyRevenue(src);
        }

        public List<Tuple<string, decimal>> getMonthlyRevenue(DateTime src)
        {
            return StatisticsDAO.Instance.getMonthlyRevenue(src);
        }

        public List<Tuple<string, decimal>> getYearlyRevenue()
        {
            return StatisticsDAO.Instance.getYearlyRevenue();
        }

        public List<Tuple<string, decimal>> getDailyProfit(DateTime src)
        {
            return StatisticsDAO.Instance.getDailyProfit(src);
        }

        public List<Tuple<string, decimal>> getWeeklyProfit(DateTime src)
        {
            return StatisticsDAO.Instance.getWeeklyProfit(src);
        }

        public List<Tuple<string, decimal>> getMonthlyProfit(DateTime src)
        {
            return StatisticsDAO.Instance.getMonthlyProfit(src);
        }

        public List<Tuple<string, decimal>> getYearlyProfit()
        {
            return StatisticsDAO.Instance.getYearlyProfit();
        }

        public List<Tuple<string, int>> getDailyQuantityOfSpecificProduct(int srcPhoneID, int srcCategoryID, DateTime srcDate)
        {
            return StatisticsDAO.Instance.getDailyQuantityOfSpecificProduct(srcPhoneID, srcCategoryID, srcDate);
        }

        public List<Tuple<string, int>> getWeeklyQuantityOfSpecificProduct(int srcPhoneID, int srcCategoryID, DateTime srcDate)
        {
            return StatisticsDAO.Instance.getWeeklyQuantityOfSpecificProduct(srcPhoneID, srcCategoryID, srcDate);
        }

        public List<Tuple<string, int>> getMonthlyQuantityOfSpecificProduct(int srcPhoneID, int srcCategoryID, DateTime srcDate)
        {
            return StatisticsDAO.Instance.getMonthlyQuantityOfSpecificProduct(srcPhoneID, srcCategoryID, srcDate);
        }

        public List<Tuple<string, int>> getYearlyQuantityOfSpecificProduct(int srcPhoneID, int srcCategoryID)
        {
            return StatisticsDAO.Instance.getYearlyQuantityOfSpecificProduct(srcPhoneID, srcCategoryID);
        }

        public List<Tuple<string, int>> getPhoneQuantityInCategory(int srcCategoryID)
        {
            return StatisticsDAO.Instance.getPhoneQuantityInCategory(srcCategoryID);
        }
    }
}
