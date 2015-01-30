using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOMNI.SDK.Public.Models.Catalog
{
    public class AutoCompleteResultItem
    {
        public int Id { get; set; }
        public string SearchType { get; set; }
        public string FullText { get; set; }
    }
}
