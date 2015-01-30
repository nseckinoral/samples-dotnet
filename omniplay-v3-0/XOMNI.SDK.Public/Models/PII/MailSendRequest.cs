using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOMNI.SDK.Public.Models.PII
{
    public class MailSendRequest
    {
        public string MailTemplateName { get; set; }
        public Guid UniqueKey { get; set; }
        public string To { get; set; }
        public string ToDisplayName { get; set; }
    }
}
