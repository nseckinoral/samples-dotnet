using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOMNI.SDK.Public.Models.Catalog
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public int SubCategoryCount { get; set; }

        public IEnumerable<Metadata> CategoryMetadata { get; set; }
        public int? ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; }

        public List<ImageAsset> ImageAssets { get; set; }
        public List<ImageAsset> DocumentAssets { get; set; }
        public List<ImageAsset> VideoAssets { get; set; }

    }
}
