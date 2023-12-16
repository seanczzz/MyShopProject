using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DTO
{
    public class Order : INotifyPropertyChanged, ICloneable
    {
        public enum OrderStatusEnum
        {
            Pending = 0,
            InProgress = 1,
            Complete = 2
        }

        public static List<string> GetAllStatusValues()
        {
            var values = new List<string>();
            foreach (OrderStatusEnum status in Enum.GetValues(typeof(OrderStatusEnum)))
                values.Add(status.ToString());
            return values;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public int ID { get; set; }
        public string CustomerName { get; set; }
        public DateOnly OrderDate { get; set; }
        public OrderStatusEnum Status { get; set; }
        public string Address { get; set; }
        //public Voucher? Voucher { get; set; }
        public BindingList<OrderDetails> OrderDetailsList { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
