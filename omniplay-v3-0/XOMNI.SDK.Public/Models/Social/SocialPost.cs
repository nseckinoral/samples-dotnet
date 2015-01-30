using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOMNI.SDK.Public.Models.Social
{
    public class SocialPost
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Content { get; set; }
        public int RelatedItemId { get; set; }
        public List<SocialComment> Comments { get; set; }
        public List<AvailableSocialPlatform> AvailablePlatforms { get; set; }
    }
}
