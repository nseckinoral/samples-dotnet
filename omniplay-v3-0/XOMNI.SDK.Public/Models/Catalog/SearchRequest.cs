using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOMNI.SDK.Public.Models.Catalog
{
    public class SearchRequest
    {
        public int? DefaultItemId { get; set; }
        public string RFID { get; set; }
        public string UUID { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public string Model { get; set; }
        public string Title { get; set; }
        public double? MinWidth { get; set; }
        public double? MaxWidth { get; set; }
        public double? MinHeight { get; set; }
        public double? MaxHeight { get; set; }
        public double? MinWeight { get; set; }
        public double? MaxWeight { get; set; }
        public double? MinDepth { get; set; }
        public double? MaxDepth { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public int? DimensionTypeId { get; set; }
        public int? WeightTypeId { get; set; }
        public int? TagId { get; set; }
        public string  DelimitedDynamicAttributeValues { get; set; }
        public bool IncludeOnlyMasterItems { get; set; }
        public string TagQuery { get; set; }
        public bool IncludePassiveItems { get; set; }
    }

}
