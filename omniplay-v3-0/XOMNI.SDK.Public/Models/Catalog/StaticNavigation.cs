using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOMNI.SDK.Public.Models.Catalog
{
    public class StaticNavigation
    {
        public List<Category> Categories { get; set; }
        public List<Brand> Brands { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Currency> Currencies { get; set; }
        public List<UnitType> UnitTypes { get; set; }
        public List<DimensionMinMax> WidthRanges { get; set; }
        public List<DimensionMinMax> HeightRanges { get; set; }
        public List<DimensionMinMax> DepthRanges { get; set; }
        public List<WeightMinMax> WeightRanges { get; set; }
        public List<PriceMinMax> PriceRanges { get; set; }
        public List<PriceMinMax> DiscountPriceRanges { get; set; }
    }
}
