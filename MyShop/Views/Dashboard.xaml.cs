using MyShop.BUS;
using MyShop.Config;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyShop.Views
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {

        public int totalPhone { get; set; } = 0;
        public int weekOrder { get; set; } = 0;
        public int monthOrder { get; set; } = 0;

        List<Phone>? phonesOutOfStock = null;
        PhoneBUS _phoneBUS = new PhoneBUS();
        OrderBUS _orderBUS = new OrderBUS();

        public Dashboard()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            totalPhone = _phoneBUS.GetTotalPhone();
            weekOrder = _orderBUS.getOrderCountByWeek();
            monthOrder = _orderBUS.getOrderCountByMonth();
            phonesOutOfStock = _phoneBUS.getTopFiveOutOfStock();
            PhoneDataGrid.ItemsSource = phonesOutOfStock;

            DataContext = this;
            AppConfig.SetValue(AppConfig.LastWindow, "Dashboard");
        }

        private void AddStockButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PhoneDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
