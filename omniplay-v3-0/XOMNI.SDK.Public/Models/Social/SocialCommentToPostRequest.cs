using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOMNI.SDK.Public.Models.Social
{
    public class SocialCommentToPostRequest
    {
        public string Content { get; set; }
        public int TargetPostId { get; set; }
    }
}
