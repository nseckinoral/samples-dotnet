using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOMNI.SDK.Public.Models.Company
{
    public class Store
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Location Location { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
