using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOMNI.SDK.Public.Models.PII
{
    public class WishlistSearchRequest
    {
        public Location Location { get; set; }
        public int TimeInterval { get; set; }
        public double MaxDistance { get; set; }
    }
}
