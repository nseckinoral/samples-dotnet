using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Target.Models
{
    public class WishlistItem
    {
        public Item Item { get; set; }
        public string BluetoothId { get; set; }
        public DateTime DateAdded { get; set; }
        public Guid UniqueKey { get; set; }
    }
}
