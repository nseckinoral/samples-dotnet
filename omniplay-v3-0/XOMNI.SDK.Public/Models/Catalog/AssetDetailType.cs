using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOMNI.SDK.Public.Models.Catalog
{
    public enum AssetDetailType
    {
        None = 0,
        IncludeOnlyDefault = 1,
        IncludeAll = 2,
        IncludeOnlyDefaultWithMetadata = 4,
        IncludeAllWithMetadata = 8
    }
}
