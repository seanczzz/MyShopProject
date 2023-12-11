using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MyShop.DTO
{
    public class Category : ICloneable, INotifyPropertyChanged
    {

        public int ID { get; set; }
        public string? CatName { get; set; }
        public BitmapImage Avatar { get; set; }
        public BindingList<Phone>? Phones { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
