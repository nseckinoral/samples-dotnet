using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOMNI.SDK.Public.Models.PII
{
    public class WishlistItem
    {
        public int ItemId { get; set; }
        public string BluetoothId { get; set; }
        public DateTime DateAdded { get; set; }
        public Guid UniqueKey { get; set; }
        public Location LastSeenLocation { get; set; }
    }
}
