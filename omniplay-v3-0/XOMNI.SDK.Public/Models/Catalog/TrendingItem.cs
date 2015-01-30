using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOMNI.SDK.Public.Models.Catalog
{
    public class TrendingItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public float TotalPoint { get; set; }
        public List<TrendingItemRecord> ItemPopularityRecords { get; set; }
    }
}
