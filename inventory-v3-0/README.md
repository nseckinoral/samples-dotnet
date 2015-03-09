# Introduction #
This is a sample project showcasing how retailers may use XOMNI InStoreMetadata API to see stock details of their products, both for a spesific store and also for other stores related to their company. Even though this document is about "stock details", InStoreMetadata API can also be used for your other needs such as [**buraya bir iki ekstra örnek gelecek**]

# APIs Used #
 - [Catalog/Item/**Fetching a Single Item**](http://dev.xomni.com/v3-0/http-api/public-apis/catalog/item/fetching-a-single-item)
 - [Catalog/Item In-Store Metadata/**Fetching the in-store metadata of an item**](http://dev.xomni.com/v3-0/http-api/public-apis/catalog/item-in-store-metadata/fetching-the-in-store-metadata-of-an-item)


**Note:**  The API version used in this sample is **v3.0**

# XOMNI SDK for .NET #


The code repository for the SDK can be found [here](https://github.com/XomniCloud/xomni-sdk-dotnet).

# XOMNI Developer Portal #

Feel free to jump into our developer portal to look for more APIs and documentation. 

[Developer Portal: Public APIs for v3.0](http://dev.xomni.com/v3-0/http-api/public-apis)

#Flow Diagram#
![](https://github.com/nseckinoral/samples-dotnet/blob/dev/inventory-v3-0/InventorySampleAppFlowDiagram.PNG?raw=true)

#How To#

## Create a New Client Context ##

First step is to create a clientcontext instance with valid credentials and a valid tenant URL, if you're using our **Public APIs**.

		ClientContext sampleClientContext = new ClientContext("UserName","Password","Service URL");
## Fetch a Single Item ##
Fetching a single item is fairly easy. All you need to do is using the **GetAsync** method of **ItemClient**.

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

After fetching the Item you desire, you can simply use the **InStoreMetadata** of it to see if the item is **in stock** or if it is, how many of it is remaining.

## Fetching the In-Store Metadata CompanyWide ##
If you need the **In-Store Metadata** of an item **for all stores**, you need to use the **GetAsync** method of **ItemInStoreMetadataClient**

**Usage:**

Fetching instore metadata has 2 different ways. First one is using a **key-value** pair, second one is using a **keyprefix**.

**Using Key-Value Pair:**

	using (ClientContext clientContext = new ClientContext("UserName","Password","Service URL"))
    {
		var itemInStoreMetadataClient = new clientContext.Of<ItemInStoreMetadataClient>();
        var metadataList = await itemInStoreMetadataClient.GetAsync(itemId, skip: 0, take: 100, key: "instock", value: "true" 
		companyWide: true);
    }

If you search with a **key-value** pair (e.g. key= "instock" value= "true"), you only get **exact matching results**. (Only in-stock items in this case.)

**Using KeyPrefix:**

	using (ClientContext clientContext = new ClientContext("UserName","Password","Service URL"))
    {
		var itemInStoreMetadataClient = new clientContext.Of<ItemInStoreMetadataClient>();
        var metadataList = await itemInStoreMetadataClient.GetAsync(itemId, skip: 0, take: 100, keyprefix: "instock", 
		companyWide: true);
    }

However, if you search with a **keyprefix** (e.g. keyprefix= **"instock"**), you can fetch **more than one key** which includes the keyprefix that you defined. (e.g. Results: key= "instock" value= "true", key= "instock_quantity" value="100" etc.)