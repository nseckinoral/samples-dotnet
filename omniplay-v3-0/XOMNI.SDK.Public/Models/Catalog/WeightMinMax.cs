using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOMNI.SDK.Public.Models.Catalog
{
    [System.Runtime.InteropServices.ComVisible(false)]
    public class WeightMinMax : MinMax<double?>
    {
        public int WeightTypeId { get; set; }
        public string WeightTypeDescription { get; set; }
    }
}
