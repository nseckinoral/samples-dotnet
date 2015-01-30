using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOMNI.SDK.Public.Models.Catalog
{
    public class ImageAsset : Asset
    {
        public List<ResizedAsset> ResizedAssets { get; set; }
    }
}
