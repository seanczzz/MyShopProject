using MyShop.Views;
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

namespace MyShop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Login login;
        Dashboard dashboard;
        Button[] buttons;
        //Dashboard dashboard;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            login = new Login();

            this.Opacity = 0.3;
            login.Owner = this;
            if (login.ShowDialog() == true )
            {
                this.Opacity = 1;
                buttons = new Button[] { dashboardButton, categoriesButton, productButton, orderButton, statsButton, configButton };
                
            }
            else
            {
                this.Close();
            }
        }

        private void changeButtonColor(Button b)
        {
            foreach (var button in buttons)
            {
                button.Background = (Brush)Application.Current.Resources["MyPinkGradient"];
            }
            b.Background = (Brush)Application.Current.Resources["MyRedGradient"];
        }

        private void dashboardButton_Click(object sender, RoutedEventArgs e)
        {
            changeButtonColor(dashboardButton);
            dashboard = new Dashboard();
            pageNavigation.NavigationService.Navigate(dashboard);
        }

        private void categoriesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void productButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void orderButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void statsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void configButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            DB.Instance.Connection?.Close();
        }
    }
}
