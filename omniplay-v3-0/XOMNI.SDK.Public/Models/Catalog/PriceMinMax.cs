using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOMNI.SDK.Public.Models.Catalog
{
    [System.Runtime.InteropServices.ComVisible(false)]
    public class PriceMinMax : MinMax<double?>
    {
        public int PriceTypeId { get; set; }
        public string PriceTypeSymbol { get; set; }
    }
}
