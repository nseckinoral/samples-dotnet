using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOMNI.SDK.Public.Models.Catalog
{
    public class SingleItemSearchResult<T> : Navigation
    {
        public T Item { get; set; }
    }
}
