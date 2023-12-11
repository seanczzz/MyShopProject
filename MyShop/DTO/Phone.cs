using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MyShop.DTO
{
    public class Phone : ICloneable, INotifyPropertyChanged
    {
        public int ID { get; set; }
        public string PhoneName { get; set; } = "";
        public string Manufacturer { get; set; } = "";
        public int Stock { get; set; }
        public string Description { get; set; } = "";
        public int BoughtPrice { get; set; }
        public int SoldPrice { get; set; }
        public Category Category { get; set; }
        public DateTime UploadDate { get; set; }
        public BitmapImage Avatar { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public object Clone()
        {
            return MemberwiseClone();
        }

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }

        private string catName;

        public string CatName { get => catName; set => SetProperty(ref catName, value); }
    }
}
