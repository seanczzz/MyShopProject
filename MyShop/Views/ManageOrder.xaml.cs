using MyShop.BUS;
using MyShop.Config;
using MyShop.DTO;
using MyShop.Utils;
using MyShop.ViewModels;
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
    /// Interaction logic for ManageOrder.xaml
    /// </summary>
    public partial class ManageOrder : Page
    {
        private OrderBUS orderBUS;

        OrderViewModel vm;
        DateTime FromDate;
        DateTime ToDate;

        int totalItems = 0;
        int currentPage = 1;
        int totalPages = 1;
        int rowsPerPage = 8;
        public ManageOrder()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            vm = new OrderViewModel();
            orderBUS = new OrderBUS();

            FromDate = DateTime.Parse("1/1/1970");
            ToDate = DateTime.MaxValue;

            LoadOrders();
            OrderDataGrid.ItemsSource = vm.OrdersOnPage;

            AppConfig.SetValue(AppConfig.LastWindow, "ManageOrder");
        }

        void LoadOrders()
        {
            vm.Orders = orderBUS.GetAllOrdersByDate(FromDate, ToDate);
            vm.OrdersOnPage = new BindingList<Order>(vm.Orders.Skip((currentPage - 1) * rowsPerPage)
                                                        .Take(rowsPerPage).ToList());

            totalItems = vm.Orders.Count();
            totalPages = totalItems / rowsPerPage + (totalItems % rowsPerPage == 0 ? 0 : 1);

            if (totalPages <= 0) totalPages = 1;
            if (currentPage > totalPages) currentPage = totalPages;

            // control prev & next buttons
            PreviousButton.IsEnabled = FirstButton.IsEnabled = currentPage > 1;
            NextButton.IsEnabled = LastButton.IsEnabled = currentPage < totalPages;

            CurrentPageText.Text = currentPage.ToString();
            TotalPageText.Text = totalPages.ToString();

            OrderDataGrid.ItemsSource = vm.OrdersOnPage;
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Order order = new Order();
            var screen = new ManageOrderDetails(order,true);
            if (screen.ShowDialog() == true)
            {
                LoadOrders();
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            int index = OrderDataGrid.SelectedIndex;
            if (index != -1)
            {
                var screen = new ManageOrderDetails(vm.OrdersOnPage[index]);
                screen.Owner = this.Parent as Window;
                var result = screen.ShowDialog();

                if (result == true)
                {
                    screen.myOrder.CopyProperties(vm.OrdersOnPage[index]);
                    orderBUS.UpdateOrder(vm.OrdersOnPage[index]);
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            int i = OrderDataGrid.SelectedIndex;

            if (i != -1)
            {
                Order order = vm.OrdersOnPage[i];
                var res = MessageBox.Show($"Are you sure to delete this order: {order.ID} - {order.CustomerName}?", "Delete order", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    orderBUS.DeleteOrder(order.ID);
                    vm.OrdersOnPage.RemoveAt(i);
                    if (vm.OrdersOnPage.Count == 0)
                    {
                        if (currentPage > 1)
                        {
                            currentPage--;
                            LoadOrders();
                        }
                    }
                }
            }
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (FromDatePicker.SelectedDate != null)
            {
                FromDate = (DateTime)FromDatePicker.SelectedDate;
            }
            else
            {
                FromDate = DateTime.Parse("1/1/1970");
            }

            if (ToDatePicker.SelectedDate != null)
            {
                ToDate = (DateTime)ToDatePicker.SelectedDate;
            }
            else
            {
                ToDate = DateTime.MaxValue;
            }

            if (FromDate <= ToDate)
            {
                LoadOrders();
            }
            else
            {
                MessageBox.Show("Start Date cannot after End Date", "Date Filter", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        
        private void DetailButton_Click(object sender, RoutedEventArgs e)
        {
            int index = OrderDataGrid.SelectedIndex;
            
            if (index != -1)
            {
                var screen = new ManageOrderDetails(vm.OrdersOnPage[index]);
                screen.Owner = this.Parent as Window;
                var result = screen.ShowDialog();

                if (result == true)
                {
                    screen.myOrder.CopyProperties(vm.OrdersOnPage[index]);
                    orderBUS.UpdateOrder(vm.OrdersOnPage[index]);
                }
            }
        }

        private void FirstButton_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;
            LoadOrders();
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            currentPage--;
            LoadOrders();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            currentPage++;
            LoadOrders();
        }

        private void LastButton_Click(object sender, RoutedEventArgs e)
        {
            currentPage = totalPages;
            LoadOrders();
        }
    }
}
