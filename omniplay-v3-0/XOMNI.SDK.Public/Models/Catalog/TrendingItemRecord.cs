using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOMNI.SDK.Public.Models.Catalog
{
    public class TrendingItemRecord
    {
        public int StoreId { get; set; }
        public string ActionType { get; set; }
        public float TotalTimeImpactValue { get; set; }
        public float TotalActionTypeImpactValue { get; set; }
        public float TotalValue { get; set; }
        public int TotalActionCount { get; set; }
    }
}
