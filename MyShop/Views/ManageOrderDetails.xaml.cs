using DocumentFormat.OpenXml.Drawing.Charts;
using MyShop.BUS;
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
using System.Windows.Shapes;
using Order = MyShop.DTO.Order;

namespace MyShop.Views
{
    /// <summary>
    /// Interaction logic for ManageOrderDetails.xaml
    /// </summary>
    public partial class ManageOrderDetails : Window
    {
        public Order myOrder { get; set; }
        private OrderBUS orderBUS;
        private bool isNewOrder = false;
        int currentOrderDetailIndex;

        //BindingList<OrderDetails> orderDetailsList;
        OrderDetails orderDetails; // To be passed to add/update phone order screen
        public ManageOrderDetails(Order order, bool isAddNewOrder = false)
        {
            InitializeComponent();
            orderBUS = new OrderBUS();
            myOrder = (Order)order.Clone();
            
            orderDetails = new OrderDetails();
            orderDetails.OrderID = myOrder.ID;

            isNewOrder = isAddNewOrder;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (myOrder.OrderDate.Equals(DateOnly.Parse(DateTime.MinValue.Date.ToShortDateString())))
                myOrder.OrderDate = DateOnly.Parse(DateTime.Now.Date.ToShortDateString());
            OrderDatePicker.SelectedDate = DateTime.Parse(myOrder.OrderDate.ToString());
            StatusComboBox.ItemsSource = Order.GetAllStatusValues();
            DataContext = myOrder;

            //if (myOrder.ID == 0)
            //{
            //    //ChoosePhoneButton.IsEnabled = false;
            //    //UpdateButton.IsEnabled = false;
            //    //DeleteButton.IsEnabled = false;
            //}

            
            if (myOrder.OrderDetailsList == null)
                myOrder.OrderDetailsList = new BindingList<OrderDetails>();

            currentOrderDetailIndex = myOrder.OrderDetailsList.Count;
            PhoneDataGrid.ItemsSource = myOrder.OrderDetailsList;
        }

        bool IsAlreadyInPhoneList(Phone phone)
        {
            if (myOrder.OrderDetailsList == null) return false;

            foreach (OrderDetails orderDetails in myOrder.OrderDetailsList) {
                if (orderDetails.Phone.ID == phone.ID) return true;
            }

            return false;
        }
        private void ChoosePhoneButton_Click(object sender, RoutedEventArgs e)
        {
            orderDetails.Phone = new Phone();
            orderDetails.Phone.PhoneName = "Choose a phone";
            orderDetails.Quantity = 0;
            var screen = new AddPhoneOrder(orderDetails);
            if (screen.ShowDialog() == true)
            {
                if (!IsAlreadyInPhoneList(screen.newOrderDetails.Phone))
                {                   
                    myOrder.OrderDetailsList.Add(screen.newOrderDetails);
                }
                else
                {
                    MessageBox.Show($"{screen.newOrderDetails.Phone.PhoneName}'s already exists in detail order.\nChoose 'Update' instead", "Duplicate phone", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                //Reload();
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            int i = PhoneDataGrid.SelectedIndex;

            if (i != -1)
            {
                var screen = new AddPhoneOrder(myOrder.OrderDetailsList[i]);
                if (screen.ShowDialog() == true)
                {
                    if (screen.newOrderDetails.Phone.ID == myOrder.OrderDetailsList[i].Phone.ID)
                    {
                        if (screen.newOrderDetails.Quantity == myOrder.OrderDetailsList[i].Quantity) return;

                        orderBUS.UpdateOrderDetail(myOrder.OrderDetailsList[i].Phone.ID, myOrder.OrderDetailsList[i]);
                    }
                    if (!IsAlreadyInPhoneList(screen.newOrderDetails.Phone))
                    {
                        int oldPhoneID = myOrder.OrderDetailsList[i].Phone.ID;
                        screen.newOrderDetails.CopyProperties(myOrder.OrderDetailsList[i]);
                        orderBUS.UpdateOrderDetail(oldPhoneID, myOrder.OrderDetailsList[i]);
                    }
                    else
                    {
                        MessageBox.Show($"{screen.newOrderDetails.Phone.PhoneName}'s already exists in detail order.\nPlease choose another phone", "Duplicate phone", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            int i = PhoneDataGrid.SelectedIndex;

            if (i != -1)
            {
                var res = MessageBox.Show($"Are you sure to discard this phone: {myOrder.OrderDetailsList[i].Phone.PhoneName}?", "Delete phone from order", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    if (i < currentOrderDetailIndex)
                    {
                        orderBUS.DeleteOrderDetail(myOrder.OrderDetailsList[i]);
                    }
                    myOrder.OrderDetailsList.RemoveAt(i);
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrderDatePicker.SelectedDate != null)
                myOrder.OrderDate = DateOnly.Parse(OrderDatePicker.SelectedDate.Value.Date.ToShortDateString());

            if (isNewOrder)
            {
                orderBUS.AddOrder(myOrder);

            }

            for (int i = currentOrderDetailIndex; i < myOrder.OrderDetailsList.Count; i++)
            {
                myOrder.OrderDetailsList[i].OrderID = myOrder.ID;
                orderBUS.AddOrderDetail(myOrder.OrderDetailsList[i]);
            }
            DialogResult = true;

        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;

        }
    }
}
