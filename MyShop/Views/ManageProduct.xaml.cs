using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Win32;
using MyShop.BUS;
using MyShop.Config;
using MyShop.DTO;
using MyShop.Utils;
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
using Category = MyShop.DTO.Category;
using Page = System.Windows.Controls.Page;

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
                    newPhone.Category = categories![categoryIndex];
                    newPhone.CatID = newPhone.Category.ID;
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
            int index = phonesListView.SelectedIndex;

            var screen = new EditPhone(vm.PhonesOnPage[index]);
            var result = screen.ShowDialog();
            if (result == true)
            {
                Phone updatedPhone = screen.updatedPhone;
                try
                {
                    phoneBus.updatePhone(updatedPhone.ID, updatedPhone);
                    updatedPhone.CopyProperties(vm.PhonesOnPage[index]);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception here");
                    MessageBox.Show(screen, ex.Message);
                }
            }
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

        private string getStringValueFromCell(string coord, SharedStringTablePart stringTable, IEnumerable<Cell> cells)
        {
            Cell nameCell = cells.FirstOrDefault(
                        c => c?.CellReference == $"{coord}"
                    )!;

            if (nameCell == null) return "";

            string stringId = nameCell!.InnerText;
            string value = stringTable.SharedStringTable
                    .ElementAt(int.Parse(stringId))
                    .InnerText;
            return value;
        }
        private int getIntValueFromCell(string coord, SharedStringTablePart stringTable, IEnumerable<Cell> cells)
        {
            Cell nameCell = cells.FirstOrDefault(
                        c => c?.CellReference == $"{coord}"
                    )!;

            if (nameCell == null) return -1;
            int value = Int32.Parse(nameCell!.InnerText);
            return value;
        }
        DateTime getDateTimeValueFromCell(string coord, SharedStringTablePart stringTable, IEnumerable<Cell> cells)
        {
            Cell nameCell = cells.FirstOrDefault(
                        c => c?.CellReference == $"{coord}"
                    )!;

            if (nameCell == null) return DateTime.Now;
            DateTime value = DateTime.Parse(nameCell!.InnerText);
            return value;
        }
        private void ImportButton_Click(object sender, RoutedEventArgs e) 
        {

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var screen = new OpenFileDialog();

            if (screen.ShowDialog() == false) return;

            string filename = screen.FileName;
            var document = SpreadsheetDocument.Open(filename, false);
            var wbPart = document.WorkbookPart!;
            var sheets = wbPart.Workbook.Descendants<Sheet>()!;

            var categoryBUS = new CategoryBUS();
            foreach (var sheet in sheets)
            {
                var wsPart = (WorksheetPart)(wbPart!.GetPartById(sheet.Id!));
                var stringTable = wbPart
                        .GetPartsOfType<SharedStringTablePart>()
                        .FirstOrDefault()!;
                var cells = wsPart.Worksheet.Descendants<Cell>();


                Category category = new Category()
                {
                    CatName = sheet.Name,
                    Phones = new BindingList<Phone>()
                };
                bool exist = categoryBUS.InsertCategory(category);
                if (!exist)
                {
                    if (categories == null) categories = new BindingList<Category>();
                    categories.Add(category);
                }

                int row = 4;
                Cell idCell;

                do
                {
                    idCell = cells.FirstOrDefault(
                    c => c?.CellReference == $"B{row}"
                    )!;

                    if (idCell?.InnerText.Length > 0)
                    {
                        string phoneName = getStringValueFromCell($"C{row}", stringTable, cells);
                        string manufacturer = getStringValueFromCell($"D{row}", stringTable, cells);
                        int boughtPrice = getIntValueFromCell($"E{row}", stringTable, cells);
                        int soldPrice = getIntValueFromCell($"F{row}", stringTable, cells);
                        int stock = getIntValueFromCell($"G{row}", stringTable, cells);
                        string description = getStringValueFromCell($"H{row}", stringTable, cells);
                        DateTime uploadDate = getDateTimeValueFromCell($"I{row}", stringTable, cells);
                        string avatar = getStringValueFromCell($"J{row}", stringTable, cells);

                        Phone importedPhone = new()
                        {
                            PhoneName = phoneName,
                            Manufacturer = manufacturer,
                            BoughtPrice = boughtPrice,
                            SoldPrice = soldPrice,
                            Stock = stock,
                            Description = description,
                            UploadDate = uploadDate,
                            Avatar = new BitmapImage(new Uri(avatar, UriKind.Absolute)),
                            CatID = category.ID
                        };

                        phoneBus.AddPhone(importedPhone);
                        foreach (var cat in categories!)
                        {
                            if (cat.ID == importedPhone.CatID)
                            {
                                cat.Phones.Add(importedPhone);
                            }
                    }
                    }
                    row++;
                } while (idCell?.InnerText.Length > 0);
            }

            loadPhones();
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e) 
        {
            float fromPrice = float.Parse(fromTextbox.Text);
            float toPrice = float.Parse(toTextbox.Text);
            if (fromPrice >= 0 && toPrice > 0 && fromPrice < toPrice)
            {
                currentPage = 1;
                nextButton.IsEnabled = true;
                previousButton.IsEnabled = false;

                vm.PhonesOnPage.Clear();
                BindingList<Phone> phones = new BindingList<Phone>();
                foreach (Phone phone in vm.Phones)
                {
                    if (phone.SoldPrice >= fromPrice && phone.SoldPrice <= toPrice)
                    {
                        phones.Add(phone);
                    }
                }

                if (phones.Count <= 0)
                {
                    MessageBox.Show("Product not found!");
                    return;
                }
                vm.PhonesOnPage = new BindingList<Phone>(phones
                                .Skip((currentPage - 1) * itemsPerPage)
                                .Take(itemsPerPage).ToList());

                currentPage = 1;
                totalItems = phones.Count;
                totalPages = (totalItems / itemsPerPage) + (totalItems % itemsPerPage == 0 ? 0 : 1);
                phonesListView.ItemsSource = vm.PhonesOnPage;
                currentPagingTextBlock.Text = $"{currentPage}/{totalPages}";

                if (totalPages <= 1)
                {
                    nextButton.IsEnabled = false;
                }
            }
            else
            {

                MessageBox.Show("Product not found!");
            }
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search_text = searchTextBox.Text;
            if (search_text.Length > 0)
            {
                currentPage = 1;
                nextButton.IsEnabled = true;
                previousButton.IsEnabled = false;

                vm.PhonesOnPage.Clear();
                BindingList<Phone> phones = new BindingList<Phone>();
                foreach (Phone phone in vm.Phones)
                {
                    if (phone.PhoneName.ToLower().Contains(search_text.ToLower()))
                    {
                        phones.Add(phone);
                    }
                }

                vm.PhonesOnPage = new BindingList<Phone>(phones
                                .Skip((currentPage - 1) * itemsPerPage)
                                .Take(itemsPerPage).ToList());

                currentPage = 1;
                totalItems = phones.Count;
                totalPages = totalItems / itemsPerPage + (totalItems % itemsPerPage == 0 ? 0 : 1);
                phonesListView.ItemsSource = vm.PhonesOnPage;
                currentPagingTextBlock.Text = $"{currentPage}/{totalPages}";
                if (totalPages <= 1)
                {
                    nextButton.IsEnabled = false;
                }
            }
            else
            {
                loadPhones();
            }
        }

        private void categoriesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            previousButton.IsEnabled = false;
            loadPhones();
        }
    }
}
