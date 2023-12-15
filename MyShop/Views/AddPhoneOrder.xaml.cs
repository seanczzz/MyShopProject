using MyShop.BUS;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyShop.Views
{
    /// <summary>
    /// Interaction logic for AddPhoneOrder.xaml
    /// </summary>
    public partial class AddPhoneOrder : Window
    {

        BindingList<Category> categories;
        static List<Phone> phoneList;
        public OrderDetails newOrderDetails { get; set; }
        public AddPhoneOrder(OrderDetails orderDetails)
        {
            InitializeComponent();
            newOrderDetails = (OrderDetails)orderDetails.Clone();

            if (phoneList == null)
            {
                phoneList = PhoneBUS.Instance.getAllPhones();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            categories = CategoryBUS.Instance.getAllCategories();

            categoryCombobox.ItemsSource = categories;

            categoryCombobox.SelectedIndex = 0;

            foreach (Category category in categories)
            {
                category.Phones = new BindingList<Phone>();
                foreach (Phone phone in phoneList)
                {
                    if (category.ID == phone.CatID)
                    {
                        category.Phones.Add(phone);
                    }
                }
            }

            PhoneListView.ItemsSource = categories[categoryCombobox.SelectedIndex].Phones;

            DataContext = newOrderDetails;
        }

        private void categoryCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PhoneListView.ItemsSource = categories[categoryCombobox.SelectedIndex].Phones;
        }

        private void PhoneListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Phone selectedPhone = (Phone)PhoneListView.SelectedItem;
            if (selectedPhone != null)
            {
                newOrderDetails.Phone = selectedPhone;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var QuantityTextBox = e.OriginalSource as TextBox;

            if (QuantityTextBox != null)
            {
                if (QuantityTextBox.Text == "")
                {
                    QuantityTextBox.Text = "0";
                }
                else if ((int.Parse(QuantityTextBox.Text)
                    > newOrderDetails.Phone.Stock))
                {
                    QuantityTextBox.Text = QuantityTextBox.Text.Remove(QuantityTextBox.Text.Length - 1);

                    if (int.Parse(QuantityTextBox.Text)
                        > newOrderDetails.Phone.Stock)
                        QuantityTextBox.Text = newOrderDetails.Phone.Stock.ToString();
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            newOrderDetails.Quantity = int.Parse(QuantityTextBox.Text);
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private new void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+$");

            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
