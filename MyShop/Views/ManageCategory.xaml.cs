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
    /// Interaction logic for ManageCategory.xaml
    /// </summary>
    public partial class ManageCategory : Page
    {
        CategoryViewModel categoryViewModel = new CategoryViewModel();
        private CategoryBUS categoryBUS = new CategoryBUS();
        public ManageCategory()
        {
            InitializeComponent();
            categoryViewModel.Categories = categoryBUS.getAllCategories();
            categoriesListView.ItemsSource = categoryViewModel.Categories;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new AddCategory();
            var result = screen.ShowDialog();
            if (result == true)
            {
                var newCategory = screen.newCategory;

                try
                {
                    categoryBUS.InsertCategory(newCategory);
                    categoryViewModel.Categories.Add(newCategory);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(screen, ex.Message);
                }

            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            int index = categoriesListView.SelectedIndex;
            Category selectedCategory = categoryViewModel.Categories[index];

            var wannaDelete = MessageBox.Show($"Bạn thật sự muốn xóa hãng điện thoại {selectedCategory.CatName}?",
                "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (wannaDelete == MessageBoxResult.Yes)
            {
                categoryBUS.DeleteCategory(selectedCategory.ID);
                categoryViewModel.Categories.RemoveAt(index);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Category selectedCategory = (Category)categoriesListView.SelectedItem;

            var screen = new EditCategory(selectedCategory);
            var result = screen.ShowDialog();
            if (result == true)
            {
                var info = screen.updatedCategory;
                selectedCategory.CatName = info.CatName;
                selectedCategory.Avatar = info.Avatar;
                try
                {
                    categoryBUS.UpdateCategory(selectedCategory);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception here");
                    MessageBox.Show(screen, ex.Message);
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AppConfig.SetValue(AppConfig.LastWindow, "ManageCategory");
        }
    }
}
