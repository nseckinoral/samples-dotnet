using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients;
using XOMNI.SDK.Public.Models;
using XOMNI.SDK.Public.Models.Catalog;

namespace XOMNI.SDK.Public.Clients.Catalog
{
	public class CategoryClient : BaseClient
	{
		public CategoryClient(HttpClient httpClient)
			: base(httpClient)
		{

		}

        public async Task<ApiResponse<CategoryTree<CategoryTreeItem>>> GetAsync(bool includeMetadata, AssetDetailType imageAssetDetail, AssetDetailType videoAssetDetail, AssetDetailType documentAssetDetail)
		{
            string path = string.Format("/catalog/categories?includeMetadata={0}&imageAssetDetail={1}&videoAssetDetail={2}&documentAssetDetail={3}", includeMetadata, (int)imageAssetDetail, (int)videoAssetDetail, (int)documentAssetDetail);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<CategoryTree<CategoryTreeItem>>>().ConfigureAwait(false);
			}
		}

        public async Task<ApiResponse<List<Category>>> GetAsync(int parentCategoryId, bool includeMetadata, AssetDetailType imageAssetDetail, AssetDetailType videoAssetDetail, AssetDetailType documentAssetDetail)
		{
            if(parentCategoryId<=0)
            {
                throw new ArgumentException("parentCategoryId");
            }

            string path = string.Format("/catalog/categories?parentCategoryId={0}&includeMetadata={1}&imageAssetDetail={2}&videoAssetDetail={3}&documentAssetDetail={4}", parentCategoryId, includeMetadata, (int)imageAssetDetail, (int)videoAssetDetail, (int)documentAssetDetail);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<List<Category>>>().ConfigureAwait(false);
			}
		}
	}
}