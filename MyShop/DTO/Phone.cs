using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DTO
{
    internal class Phone
    {
        public int ID { get; set; }
        public string PhoneName { get; set; } = "";
        public string Manufacturer { get; set; } = "";
        public int Stock { get; set; }
        public string Description { get; set; } = "";
        public int BoughtPrice { get; set; }
        public int SoldPrice { get; set; }
    }
}
