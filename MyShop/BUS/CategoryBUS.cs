using MyShop.DAO;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyShop.BUS
{
    internal class CategoryBUS
    {
        private CategoryDAO categoryDAO;
        public CategoryBUS()
        {
            categoryDAO = new CategoryDAO();
        }

        public BindingList<Category> getAllCategories()
        {
            return categoryDAO.getAllCategories();
        }

        public void InsertCategory(Category category)
        {
            categoryDAO.InsertCategory(category);
        }

        public void DeleteCategory(int id) { 
            categoryDAO.DeleteCategory(id);
        }

        public void UpdateCategory(Category category)
        {
           categoryDAO.updateCategory(category);
        }
    }
}
