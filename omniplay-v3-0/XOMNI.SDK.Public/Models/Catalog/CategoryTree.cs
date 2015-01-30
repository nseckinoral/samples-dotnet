using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOMNI.SDK.Public.Models.Catalog
{
    public class CategoryTree<T> where T : CategoryTreeItem
    {
        public List<T> Categories { get; set; }
    }

}
