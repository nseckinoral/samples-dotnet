using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOMNI.SDK.Public.Models.PII
{
    public class AnonymousUser : User
    {
        public override string PIIUserType
        {
            get
            {
                return "Anonymous";
            }
        }
    }
}
