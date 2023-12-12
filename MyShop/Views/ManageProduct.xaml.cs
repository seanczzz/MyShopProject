using MyShop.BUS;
using MyShop.Config;
using MyShop.DTO;
using MyShop.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    /// Interaction logic for ManageProduct.xaml
    /// </summary>
    public partial class ManageProduct : Page
    {
        private PhoneBUS phoneBus = new PhoneBUS();
        PhoneViewModel vm = new PhoneViewModel();
        static BindingList<Category>? categories = null;
        int totalItems = 0;
        int currentPage = 1;
        int totalPages = 0;
        int itemsPerPage = int.Parse(AppConfig.GetValue(AppConfig.NumberProductPerPage));
        int i = 0;
        public ManageProduct()
        {
            InitializeComponent();

            previousButton.IsEnabled = false;
            nextButton.IsEnabled = false;
            //currentPage = 0;
            //totalPages = 0;
            var categoryBUS = new CategoryBUS();
            var phoneBUS = new PhoneBUS();
            if (categories == null)
            {
                categories = categoryBUS.getAllCategories();
            }
            categoriesListView.ItemsSource = categories;

            var allPhones = phoneBUS.getAllPhones();

            foreach (var category in categories)
            {
                if (category.Phones == null)
                {
                    category.Phones = new BindingList<Phone>();
                    foreach(var phone in allPhones)
                    {
                        if (phone.CatID == category.ID)
                        {
                            category.Phones.Add(phone);
                        }
                    }
                }
            }

            if (categories.Count > 0)
            {
                loadPhones();
            }

            AppConfig.SetValue(AppConfig.LastWindow, "ManageProduct");
        }


        void loadPhones()
        {
            i = categoriesListView.SelectedIndex;
            if (i < 0)
            {
                i = 0;
            }
            if (categories == null)
            {
                return;
            }
            currentPage = 1;
            previousButton.IsEnabled = false;
            vm.Phones = categories[i].Phones!;
            vm.PhonesOnPage = new BindingList<Phone>(vm.Phones.Skip((currentPage - 1) * itemsPerPage)
                                                                .Take(itemsPerPage)
                                                                .ToList());

            totalItems = vm.Phones.Count;
            totalPages = (totalItems / itemsPerPage) + (totalItems % itemsPerPage == 0 ? 0 : 1);
            currentPage = totalPages > 0 ? 1 : 0;
            currentPagingTextBlock.Text = $"{currentPage}/{totalPages}";

            if (totalPages > 1)
            {
                nextButton.IsEnabled = true;
            }
            else
            {
                nextButton.IsEnabled = false;
            }

            phonesListView.ItemsSource = vm.PhonesOnPage;
        }


        private void AddMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var screen = new AddPhone(categories!);
            var result = screen.ShowDialog();
            if (result == false) return;
            var newPhone = screen.newPhone;
            var categoryIndex = screen.categoryIndex;
            if (categoryIndex >= 0)
            {
                try
                {
                    newPhone.CatID = categoryIndex + 1;
                    newPhone.Category = categories![categoryIndex];
                    phoneBus.AddPhone(newPhone);
                    categories![categoryIndex].Phones.Add(newPhone);
                    if (i == categoryIndex)
                    {
                        vm.PhonesOnPage.Add(newPhone);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(screen, ex.Message);
                }
            }
        }
        private void editMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //var p = (Phone)phonesListView.SelectedItem;
            //var screen = new EditPhone(p);
            //var result = screen.ShowDialog();
            //if (result == true)
            //{
            //    var info = screen.EditedPhone;
            //    p.PhoneName = info.PhoneName;
            //    p.Manufacturer = info.Manufacturer;
            //    p.SoldPrice = info.SoldPrice;
            //    p.BoughtPrice = info.BoughtPrice;
            //    p.Description = info.Description;
            //    p.Avatar = info.Avatar;
            //    p.Stock = info.Stock;
            //    try
            //    {
            //        phoneBus.updatePhone(p.ID, p);
            //        searchTextBox_TextChanged(sender, null);
            //    }
            //    catch (Exception ex)
            //    {
            //        Debug.WriteLine("Exception here");
            //        MessageBox.Show(screen, ex.Message);
            //    }

            //    //_vm.Phones = _categories[i].Phones;
            //    //_vm.SelectedPhones = _vm.Phones
            //    //    .Skip((_currentPage - 1) * _rowsPerPage)
            //    //    .Take(_rowsPerPage).ToList();

            //    //phonesListView.ItemsSource = _vm.SelectedPhones;
            //}
        }

        private void deleteMenuItem_Click(object sender, RoutedEventArgs e) 
        {
            int index = phonesListView.SelectedIndex;
            Phone selectedPhone = vm.PhonesOnPage[index];

            var wannaDelete = MessageBox.Show($"Bạn thật sự muốn xóa điện thoại {selectedPhone.PhoneName}?",
                "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (wannaDelete == MessageBoxResult.Yes)
            {
                phoneBus.DeletePhone(selectedPhone.ID);
                vm.Phones.RemoveAt((currentPage - 1) * itemsPerPage + index);
                vm.PhonesOnPage.RemoveAt(index);
                if (vm.PhonesOnPage.Count == 0)
                {
                    loadPhones();
                }
            }
        }

        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            nextButton.IsEnabled = true;
            currentPage--;
            vm.PhonesOnPage = new BindingList<Phone>(vm.Phones.Skip((currentPage - 1) * itemsPerPage)
                                                                .Take(itemsPerPage)
                                                                .ToList());

            // ép cập nhật giao diện
            phonesListView.ItemsSource = vm.PhonesOnPage;
            currentPagingTextBlock.Text = $"{currentPage}/{totalPages}";
            if (currentPage <= 1)
            {
                previousButton.IsEnabled = false;
            }
        }

        private void nextButton_Click(object sender, RoutedEventArgs e) 
        {
            previousButton.IsEnabled = true;
            currentPage++;
            vm.PhonesOnPage = new BindingList<Phone>(vm.Phones.Skip((currentPage - 1) * itemsPerPage)
                                                                .Take(itemsPerPage)
                                                                .ToList());

            // ép cập nhật giao diện
            phonesListView.ItemsSource = vm.PhonesOnPage;
            currentPagingTextBlock.Text = $"{currentPage}/{totalPages}";

            if (currentPage >= totalPages)
            {
                nextButton.IsEnabled = false;
            }
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e) { }

        private void FilterButton_Click(object sender, RoutedEventArgs e) { }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void categoriesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            previousButton.IsEnabled = false;
            loadPhones();
        }
    }
}
