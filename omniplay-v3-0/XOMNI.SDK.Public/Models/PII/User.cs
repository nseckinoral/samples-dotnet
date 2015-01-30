using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOMNI.SDK.Public.Models.PII
{
    public class User
    {
        public virtual string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public virtual string PIIUserType { get; set; }
    }
}
