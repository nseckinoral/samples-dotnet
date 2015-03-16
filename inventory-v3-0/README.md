# Introduction #
This is a sample project showcasing how you can use [InStoreMetadata APIs](http://dev.xomni.com/v3-0/http-api/public-apis/catalog/item-in-store-metadata) to see stock details of products for a spesific store or look-up across multiple stores for availability. Even though in this document we are only going to discuss inventory aspect of our APIs, InStoreMetadata APIs are designed in a generic way to give you flexibility implementing different scenarios where you might need store specific contextual data.

# APIs Used #
 - [Catalog/Item/**Fetching a Single Item**](http://dev.xomni.com/v3-0/http-api/public-apis/catalog/item/fetching-a-single-item)
 - [Catalog/Item In-Store Metadata/**Fetching the in-store metadata of an item**](http://dev.xomni.com/v3-0/http-api/public-apis/catalog/item-in-store-metadata/fetching-the-in-store-metadata-of-an-item)


**Note:**  The API version used in this sample is **v3.0**

# XOMNI SDK for .NET #


The code repository for the .NET SDK used in this article can be found [here](https://github.com/XomniCloud/xomni-sdk-dotnet).

# XOMNI Developer Portal #

Feel free to jump into our developer portal to look for more APIs and related documentation. 

[Developer Portal: Public APIs for v3.0](http://dev.xomni.com/v3-0/http-api/public-apis)
# What is "In-Store Metadata?" #
In-Store Metadata is a **store based** metadata that may be used to store contextual and optional data about an item.

In "**Inventory Sample App**", it is used in 2 different ways:

- Fetch a single item, check if it's available to buy.
- If it's not available, see other stores the product can be bought from.

To see other stores available, first we fetched In-Store metadata of the item across multiple stores. Then we fetched nearby stores. After all, we took the matching Store IDs and presented them in a map control.

#Flow Diagram#
![](https://github.com/nseckinoral/samples-dotnet/blob/dev/inventory-v3-0/InventorySampleAppFlowDiagram.PNG?raw=true)

#How To#

## Create a New Client Context ##

First step is to create a clientcontext instance with valid credentials and a valid tenant URL.

		ClientContext sampleClientContext = new ClientContext("UserName","Password","Service URL");
## Fetch a Single Item ##
Fetching a single item is fairly easy. All you need to do is to use the **GetAsync** method of **ItemClient**.

**Note:** If you're going to fetch a single item to see InStore Metadata of an item for **your store**, make sure the Store ID associated with your **license** is defined in the InStore Metadata.

**Usage:**

	    using (ClientContext clientContext = new ClientContext("UserName","Password","Service URL"))
	    {
	        var itemClient = new clientContext.Of<ItemClient>();
            var sampleItem = await itemClient.GetAsync(ItemId, includeItemInStoreMetadata :true, imageAssetDetail: AssetDetailType.IncludeOnlyDefault);
	    }


Make sure you set "**includeItemInStoreMetadata**" paramater to **true** as we're going to use InStoreMetadata of the item to see stock details.

**Possible Enumarable Values for AssetDetailTypes:**

 - **0** : Default Value – Image assets are not included in response.
 - **1** : Includes only default image asset.
 - **2** : Includes only default image asset with metadata
 - **4** : Includes all image assets
 - **8** : Includes all image assets with metadata

After fetching the Item you desire, you can simply use the **InStoreMetadata** of the item to see if the product is **in stock**. 

**Example Response:**
	


	"InStoreMetadata":[
				{
					"Key":"instock",
					"Value":"false"
				}],

## Fetching the In-Store Metadata Across Multiple Stores ##
If you need the **In-Store Metadata** of an item **for all stores**, you need to use the **GetAsync** method of **ItemInStoreMetadataClient**.

**Usage:**

Fetching in-store metadata has two different ways. You can use a **key-value** pair or a **keyprefix**.

**Using Key-Value Pair:**

	using (ClientContext clientContext = new ClientContext("UserName","Password","Service URL"))
    {
		var itemInStoreMetadataClient = new clientContext.Of<ItemInStoreMetadataClient>();
        var metadataList = await itemInStoreMetadataClient.GetAsync(itemId, skip: 0, take: 100, key: "instock", value: "true" 
		companyWide: true);
    }

If your search includes a **key-value** pair (e.g. key= "instock" value= "true"), you will only get **exact matching results**.

**Example Response:**

	{
		"Data":[
			{
				"StoreId":1,
				"Key":"instock",
				"Value":"true"
			},
			{
				"StoreId":2,
				"Key":"instock",
				"Value":"true"
			},
			{
				"StoreId":3,
				"Key":"instock",
				"Value":"true"
			}
		]
	}

----------


**Using KeyPrefix:**

	using (ClientContext clientContext = new ClientContext("UserName","Password","Service URL"))
    {
		var itemInStoreMetadataClient = new clientContext.Of<ItemInStoreMetadataClient>();
        var metadataList = await itemInStoreMetadataClient.GetAsync(itemId, skip: 0, take: 100, keyprefix: "instock", 
		companyWide: true);
    }

However, if you search with a **keyprefix** (e.g. keyprefix= **"instock"**), you can fetch **more than one key** including your keyprefix.


**Example Response:**

	{
		"Data":[
			{
				"StoreId":1,
				"Key":"instock",
				"Value":"true"
			},
			{
				"StoreId":1,
				"Key":"instock_quantity",
				"Value":"25"
			},
			{
				"StoreId":2,
				"Key":"instock",
				"Value":"true"
			},
			{
				"StoreId":3,
				"Key":"instock",
				"Value":"true"
			},
			{
				"StoreId":4,
				"Key":"instock",
				"Value":"false"
			},
			
			{	
				"StoreId":2,
				"Key":"instock_quantity",
				"Value":"25"
			},
			{
				"StoreId":3,
				"Key":"instock_quantity",
				"Value":"40"
			}
		]
	}