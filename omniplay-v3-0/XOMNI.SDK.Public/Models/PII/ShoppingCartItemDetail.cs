using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Models.Catalog;

namespace XOMNI.SDK.Public.Models.PII
{
    public class ShoppingCartItemDetail
    {
        public List<Metadata> CategoryMetadata { get; set; }
        public int Quantity { get; set; }
        public Item Item { get; set; }
        public string BluetoothId { get; set; }
        public DateTime DateAdded { get; set; }
        public Location LastSeenLocation { get; set; }
        public Guid UniqueKey { get; set; }
    }
}
