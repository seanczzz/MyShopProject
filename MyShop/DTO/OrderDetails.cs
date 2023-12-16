using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DTO
{
    public class OrderDetails : ICloneable, INotifyPropertyChanged
    {
        public int DetailOrderID { get; set; }
        public int OrderID { get; set; }
        public Phone Phone { get; set; }
        public int Quantity { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public object Clone()
        {
            return new OrderDetails() {
                DetailOrderID = DetailOrderID,
                OrderID = OrderID, 
                Phone = (Phone)Phone.Clone(), 
                Quantity = Quantity 
            };
        }
    }
}
