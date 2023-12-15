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
    public class CategoryBUS
    {
        private static CategoryBUS? _instance = null;

        public static CategoryBUS Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CategoryBUS();
                }

                return _instance;
            }
        }
        public BindingList<Category> getAllCategories()
        {
            return CategoryDAO.Instance.getAllCategories();
        }

        public bool InsertCategory(Category category)
        {
            bool exist = false;
            int ID = CategoryDAO.Instance.isExistCategory(category.CatName!);
            if (ID > 0)
            {
                category.ID = ID;
                exist = true;
            }
            else
            {
                CategoryDAO.Instance.InsertCategory(category);
            }
            return exist;
        }

        public void DeleteCategory(int id) {
            CategoryDAO.Instance.DeleteCategory(id);
        }

        public void UpdateCategory(Category category)
        {
            CategoryDAO.Instance.updateCategory(category);
        }
    }
}
