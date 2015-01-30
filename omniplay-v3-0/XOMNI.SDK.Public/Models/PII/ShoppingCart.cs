using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOMNI.SDK.Public.Models.PII
{
    public class ShoppingCart
    {
        public Guid UniqueKey { get; set; }
        public virtual string Name { get; set; }
        public Location LastSeenLocation { get; set; }
        public bool IsPublic { get; set; }
    }
}
