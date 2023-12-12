using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace MyShop.Views
{
    /// <summary>
    /// Interaction logic for EditPhone.xaml
    /// </summary>
    public partial class EditPhone : Window
    {
        public Phone updatedPhone { get; set; }
        public EditPhone(Phone phone)
        {
            InitializeComponent();
            updatedPhone = (Phone)phone.Clone();
            DataContext = updatedPhone;
        }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog();
            screen.Filter = "Image Files|*.jpg;*.jpeg;*.png;...";
            if (screen.ShowDialog() == true)
            {
                updatedPhone.Avatar = new BitmapImage(new Uri(screen.FileName, UriKind.Absolute));
                avatar.Source = updatedPhone.Avatar;
            }
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
