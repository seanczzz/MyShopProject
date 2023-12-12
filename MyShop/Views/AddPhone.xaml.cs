using Microsoft.Win32;
using MyShop.BUS;
using MyShop.DTO;
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

namespace MyShop.Views
{
    /// <summary>
    /// Interaction logic for AddPhone.xaml
    /// </summary>
    public partial class AddPhone : Window
    {
        public Phone newPhone { get; set; }
        public int categoryIndex { get; set; } = -1;

        public AddPhone(BindingList<Category> category)
        {
            InitializeComponent();
            newPhone = new Phone();
            DataContext = newPhone;
            categoryCombobox.ItemsSource = category;
        }

        private void categoryCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            categoryIndex = categoryCombobox.SelectedIndex;
        }

        private void chooseImageButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog();
            if (screen.ShowDialog() == true)
            {
                newPhone.Avatar = new BitmapImage(new Uri(screen.FileName, UriKind.Absolute));
                avatar.Source = newPhone.Avatar;
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (categoryIndex == -1)
            {
                MessageBox.Show(this, "Try again!!!");
            }
            else
            {
                DialogResult = true;
            }
        }
    }
}
