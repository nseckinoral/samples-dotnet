using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOMNI.SDK.Public.Models.PII
{
    public class ShoppingCartWithItems : ShoppingCart
    {
        public List<ShoppingCartItemDetail> ShoppingCartItemDetails { get; set; }
    }
}
