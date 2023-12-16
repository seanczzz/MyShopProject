using MyShop.Config;
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
        Dashboard? dashboard = null;
        ManageCategory? manageCategories = null;
        ManageProduct? manageProducts = null;
        ManageOrder? manageOrders = null;
        Statistics? statisticsPage = null;
        Configuration? configPage = null;
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


                if (AppConfig.GetValue(AppConfig.LastWindow) == "0")
                {
                    changeButtonColor(dashboardButton);
                    if (dashboard == null)
                    {
                        dashboard = new Dashboard();
                    }
                    pageNavigation.NavigationService.Navigate(dashboard);
                }
                else
                {
                    if (AppConfig.GetValue(AppConfig.LastWindow) == "ManageCategory")
                    {
                        changeButtonColor(categoriesButton);
                        if (manageCategories == null)
                        {
                            manageCategories = new ManageCategory();
                        }
                        pageNavigation.NavigationService.Navigate(manageCategories);
                    }
                    else if (AppConfig.GetValue(AppConfig.LastWindow) == "ManageOrder")
                    {
                        changeButtonColor(orderButton);
                        if (manageOrders == null)
                        {
                            manageOrders = new ManageOrder();
                        }
                        pageNavigation.NavigationService.Navigate(manageOrders);
                    }
                    else if (AppConfig.GetValue(AppConfig.LastWindow) == "Statistics")
                    {
                        changeButtonColor(statsButton);
                        if (statisticsPage == null)
                        {
                            statisticsPage = new Statistics();
                        }
                        pageNavigation.NavigationService.Navigate(statisticsPage);
                    }

                    else if (AppConfig.GetValue(AppConfig.LastWindow) == "ManageProduct")
                    {
                        changeButtonColor(productButton);
                        if (manageProducts == null)
                        {
                            manageProducts = new ManageProduct();
                        }
                        pageNavigation.NavigationService.Navigate(manageProducts);
                    }
                }
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
                
                button.Background = (Brush)Application.Current.Resources["MyGalaxyGradient"];
            }
            b.Background = (Brush)Application.Current.Resources["MyYellowGradientAdvance"];
        }

        private void dashboardButton_Click(object sender, RoutedEventArgs e)
        {
            changeButtonColor(dashboardButton);

            if (dashboard == null)
            {
                dashboard = new Dashboard();
            }
            pageNavigation.NavigationService.Navigate(dashboard);
        }

        private void categoriesButton_Click(object sender, RoutedEventArgs e)
        {
            changeButtonColor(categoriesButton);

            if (manageCategories == null)
            {
                manageCategories = new ManageCategory();
            }
            pageNavigation.NavigationService.Navigate(manageCategories);
        }

        private void productButton_Click(object sender, RoutedEventArgs e)
        {
            changeButtonColor(productButton);

            if (manageProducts == null)
            {
                manageProducts = new ManageProduct();
            }
            pageNavigation.NavigationService.Navigate(manageProducts);
        }

        private void orderButton_Click(object sender, RoutedEventArgs e)
        {
            changeButtonColor(orderButton);

            if (manageOrders == null)
            {
                manageOrders = new ManageOrder();
            }
            pageNavigation.NavigationService.Navigate(manageOrders);
        }

        private void statsButton_Click(object sender, RoutedEventArgs e)
        {
            changeButtonColor(statsButton);

            if (statisticsPage == null)
            {
                statisticsPage = new Statistics();
            }
            pageNavigation.NavigationService.Navigate(statisticsPage);
        }

        private void configButton_Click(object sender, RoutedEventArgs e)
        {
            changeButtonColor(configButton);
            if (configPage == null)
            {
                configPage = new Configuration();
            }
            pageNavigation.NavigationService.Navigate(configPage);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            DB.Instance.Connection?.Close();
        }
    }
}
