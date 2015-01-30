using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOMNI.SDK.Public.Models.Social
{
    public class SocialComment
    {
        public int TargetPostId { get; set; }
        public int TargetCommentId { get; set; }
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Content { get; set; }
        public int RelatedItemId { get; set; }
        public List<AvailableSocialPlatform> AvailablePlatforms { get; set; }
    }
}
