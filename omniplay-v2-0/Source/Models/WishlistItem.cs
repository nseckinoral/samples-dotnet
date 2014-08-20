using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source.Models
{
    public class WishlistItem
    {
        public int ItemId { get; set; }
        public string BluetoothId { get; set; }
        public Location LastSeenLocation { get; set; }
    }
}
