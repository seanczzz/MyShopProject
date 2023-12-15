using MyShop.BUS;
using MyShop.Config;
using MyShop.DTO;
using MyShop.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        BindingList<Phone>? phonesOutOfStock = null;
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            totalPhone = PhoneBUS.Instance.GetTotalPhone();
            weekOrder = OrderBUS.Instance.getOrderCountByWeek();
            monthOrder = OrderBUS.Instance.getOrderCountByMonth();

            phonesOutOfStock = new BindingList<Phone>();
            foreach (Phone phone in PhoneBUS.Instance.getTopFiveOutOfStock()) {
                phonesOutOfStock.Add(phone);
            }

            PhoneDataGrid.ItemsSource = phonesOutOfStock;

            DataContext = this;
            AppConfig.SetValue(AppConfig.LastWindow, "Dashboard");
        }

        private void AddStockButton_Click(object sender, RoutedEventArgs e)
        {
            var row = GetParent<DataGridRow>((Button)sender);
            int index = PhoneDataGrid.Items.IndexOf(row.Item);
            if (index != -1)
            {
                Phone p = phonesOutOfStock![index];
                var screen = new AddStock(p);
                var result = screen.ShowDialog();
                if (result == true)
                {
                    try
                    {
                        var newPhone = screen.newPhone;
                        PhoneBUS.Instance.updatePhone(p.ID, newPhone);

                        // reset top 5 
                        int i = 0;
                        foreach (Phone phone in PhoneBUS.Instance.getTopFiveOutOfStock())
                        {
                            phone.CopyProperties(phonesOutOfStock[i]);
                            i++;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void PhoneDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private TargetType GetParent<TargetType>(DependencyObject o) where TargetType : DependencyObject
        {
            if (o == null || o is TargetType) return (TargetType)o;
            return GetParent<TargetType>(VisualTreeHelper.GetParent(o));
        }
    }
}
