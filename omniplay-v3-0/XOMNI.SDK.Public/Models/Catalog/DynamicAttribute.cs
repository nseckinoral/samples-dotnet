using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOMNI.SDK.Public.Models.Catalog
{
    public class DynamicAttribute
    {
        public int TypeId { get; set; }
        public int TypeValueId { get; set; }
        public string Value { get; set; }
        public string TypeName { get; set; }
    }
}
