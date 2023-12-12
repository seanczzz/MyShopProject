using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.ViewModels
{
    internal class PhoneViewModel : INotifyPropertyChanged
    {
        public BindingList<Phone> Phones { get; set; }
        public BindingList<Phone> PhonesOnPage { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
