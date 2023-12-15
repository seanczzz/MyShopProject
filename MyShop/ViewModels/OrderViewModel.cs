using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.ViewModels
{
    public class OrderViewModel : INotifyPropertyChanged
    {
        public BindingList<Order> Orders { get; set; } = new BindingList<Order>();
        public BindingList<Order> OrdersOnPage { get; set; } = new BindingList<Order>();

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
