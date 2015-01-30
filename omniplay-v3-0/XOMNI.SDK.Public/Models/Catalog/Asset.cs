using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOMNI.SDK.Public.Models.Catalog
{
    public class Asset
    {
        public List<Metadata> AssetMetadata { get; set; }
        public int AssetId { get; set; }
        public string AssetUrl { get; set; }
        public bool IsDefault { get; set; }
    }
}
