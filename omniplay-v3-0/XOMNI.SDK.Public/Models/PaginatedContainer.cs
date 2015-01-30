using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOMNI.SDK.Public.Models
{
    public class PaginatedContainer<T>
    {
        public List<T> Results { get; set; }
        public int TotalCount { get; set; }
    }
}
