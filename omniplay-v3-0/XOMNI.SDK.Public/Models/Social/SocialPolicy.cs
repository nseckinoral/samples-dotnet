using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOMNI.SDK.Public.Models.Social
{
    public class SocialPolicy
    {
        public int MaxContentLength { get; set; }
        public int ShortenedUrlLength { get; set; }
        public string RepliedToTwitterAlias { get; set; }
    }
}
