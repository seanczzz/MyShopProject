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

        public bool InsertCategory(Category category)
        {
            bool exist = false;
            int ID = categoryDAO.isExistCategory(category.CatName!);
            if (ID > 0)
            {
                category.ID = ID;
                exist = true;
            }
            else
            {
                categoryDAO.InsertCategory(category);
            }
            return exist;
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
