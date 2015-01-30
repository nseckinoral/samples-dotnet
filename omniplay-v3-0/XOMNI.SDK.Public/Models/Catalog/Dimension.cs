using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOMNI.SDK.Public.Models.Catalog
{
    public class Dimension
    {
        public int DimensionTypeId { get; set; }
        public string DimensionDescription { get; set; }
        public double? Width { get; set; }
        public double? Height { get; set; }
        public double? Depth { get; set; }

        public override string ToString()
        {
            return this.Depth + "x" + this.Width + "x" + this.Height;
        }
    }
}
