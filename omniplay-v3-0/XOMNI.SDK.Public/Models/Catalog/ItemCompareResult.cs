using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOMNI.SDK.Public.Models.Catalog
{
    public class ItemCompareResult
    {
        public CompareRow[] TableRows { get; set; }
    }

    public class CompareRow
    {
        public CompareCell[] Cells { get; set; }
    }

    public class CompareCell
    {
        public string Value { get; set; }
    }

}
