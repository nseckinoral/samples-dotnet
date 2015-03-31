# Introduction #
This is a sample project showcasing how you can use [InStoreMetadata APIs](http://dev.xomni.com/v3-0/http-api/public-apis/catalog/item-in-store-metadata) to enable inventory-related scenarios in the retail industry. In our sample implementation, you can run queries for a specific store or run a look-up across multiple stores for product availability. Keep in mind that in this document we are only going to discuss inventory aspect of our APIs. InStoreMetadata APIs are designed in a generic way to give you the flexibility you need to implement different scenarios where you might need store specific contextual data.

# What is "In-Store Metadata?" #
In-Store Metadata is a **store based** metadata that may be used to store contextual and optional data about an item for a particular store.

# How to Support Inventory With In-Store Metadata APIs?#
In our "**Inventory Sample App**", the APIs are used in two different ways:

- **Fetch a single item** and check if it's available for purchase within the current store.
- If it's not available in the current store, see other stores where the product can be bought from by **fetching the in-store metadata across multiple stores**.

After fetching the in-store metadata across multiple stores, we represent them in a simple map. We have used Bing Maps for the client implementation. You can find the reference link at the bottom for more information about the client control.

Two different instore metadata have been created for the sample items in this project. One of them is to show the **item availability**, the other one is to show the **stock quantity**.

**Example Response:**

	{
	  "Data": [
	    {
	      "Metadata": [
	        {
	          "Key": "instock",
	          "Value": "true"
	        },
	        {
	          "Key": "instock_quantity",
	          "Value": "25"
	        }
	      ],
	      "Id": 1,
	      "Description": "",
	      "Location": {
	        "Longitude": -75.952559,
	        "Latitude": 40.801135
	      },
	      "Name": "XOMNI Sample Store Name",
	      "Address": "112 East 106th Street, New York"
	    }
	  ]
	}


If you **fetch a single item** to view its in-store metadata, you can do it only for the **current store** (which is associated with your license). If you want to query availability across **multiple stores**, you need to execute a **company wide** search instead.

XOMNI InStoreMetadata APIs provide **two options** for a **company wide** search. You can either use a "**key-value**" pair to fetch **exact information** you need or you can use a "**keyprefix**" to get multiple key/value pairs related to a particular store. 

While "**key-value**" pair is a **clean** way of searching, "**keyprefix**" is a more **flexible** way as you can fetch multiple metadata matching your magic word.

All steps are explained in detail below.

#Flow Diagram#
![](https://github.com/nseckinoral/samples-dotnet/blob/master/inventory-v3-0/InventorySampleAppFlowDiagram.PNG?raw=true)

#How To#

## Fetch a Single Item? ##

Fetching a single item is fairly easy. All you need to do is to use the **GetAsync** method of **ItemClient**. As always, you need to create a 
**clientcontext** instance with **valid credentials** and a **valid tenant URL** first.



**Usage:**

	    using (ClientContext clientContext = new ClientContext("UserName","Password","Service URL"))
	    {
	        var itemClient = new clientContext.Of<ItemClient>();
            var sampleItem = await itemClient.GetAsync(ItemId, includeItemInStoreMetadata :true, 
			imageAssetDetail: AssetDetailType.IncludeOnlyDefault);
	    }


Make sure you set "**includeItemInStoreMetadata**" paramater to **true** as we need to access InStoreMetadata of the item to see stock details.

**Possible Enumarable Values for AssetDetailTypes:**

 - **0** : Default Value – Image assets are not included in response.
 - **1** : Includes only default image asset.
 - **2** : Includes only default image asset with metadata
 - **4** : Includes all image assets
 - **8** : Includes all image assets with metadata

After fetching the Item you need, you can simply use the **InStoreMetadata** of the item to see if the product is **in stock**. 

**Example Response:**
	


	"InStoreMetadata":[
				{
					"Key":"instock",
					"Value":"false"
				}],

## Fetching the In-Store Metadata Across Multiple Stores ##
If you need the **In-Store Metadata** of an item **for all stores**, you need to use the **GetAsync** method of **ItemInStoreMetadataClient**. 

If you provide a location, In-Store Metadata search query will be executed on stores nearby to the given location. When you execute a location-based search, you must set **companyWide** parameter to **true**.

**Note:** Search distance is the maximum search distance in miles for a given search location.

**Usage:**

Fetching in-store metadata is possible through two different ways. You can use a **key-value** pair or a **keyprefix**.

**Using Key-Value Pair:**

	using (ClientContext clientContext = new ClientContext("UserName","Password","Service URL"))
    {
		var myLocation = new Location { Longitude = -75.952134, Latitude = 40.801112 };
		var itemInStoreMetadataClient = new clientContext.Of<ItemInStoreMetadataClient>();
		var metadataList = await itemInStoreMetadataClient.GetAsync(ItemId,key: "instock", value: "true",companyWide: true,
		location: myLocation,searchDistance: 1);
    }

If your search includes a **key-value** pair (e.g. key= "instock" value= "true"), you will only get the **exact matching results**.

**Example Response:**

	{
	  "Data": [
	    {
	      "Metadata": [
	        {
	          "Key": "instock",
	          "Value": "true"
	        }
	      ],
	      "Id": 1,
	      "Description": "",
	      "Location": {
	        "Longitude": -75.952559,
	        "Latitude": 40.801135
	      },
	      "Name": "XOMNI Sample Store Name",
	      "Address": "112 East 106th Street, New York"
	    },
	    {
	      "Metadata": [
	        {
	          "Key": "instock",
	          "Value": "true"
	        }
	      ],
	      "Id": 2,
	      "Description": "",
	      "Location": {
	        "Longitude": -75.953134,
	        "Latitude": 40.801235
	      },
	      "Name": "XOMNI Store,
	      "Address": "105 East 106th Street, New York"
	    }
	  ]
	}

----------


**Using KeyPrefix:**

	using (ClientContext clientContext = new ClientContext("UserName","Password","Service URL"))
    {
		var myLocation = new Location { Longitude = -75.952134, Latitude = 40.801112 };
		var itemInStoreMetadataClient = new clientContext.Of<ItemInStoreMetadataClient>();
		var metadataList = await itemInStoreMetadataClient.GetAsync(ItemId,keyprefix: "instock",companyWide: true,
		location: myLocation,searchDistance: 1);
    }

However, if you search with a **keyprefix** (e.g. keyprefix= **"instock"**), you can fetch **more than one key** including your keyprefix.


**Example Response:**

	{
	  "Data": [
	    {
	      "Metadata": [
	        {
	          "Key": "instock",
	          "Value": "true"
	        },
	        {
	          "Key": "instock_quantity",
	          "Value": "25"
	        }
	      ],
	      "Id": 1,
	      "Description": "",
	      "Location": {
	        "Longitude": -75.952559,
	        "Latitude": 40.801135
	      },
	      "Name": "XOMNI Sample Store Name",
	      "Address": "112 East 106th Street, New York"
	    },
	    {
	      "Metadata": [
	        {
	          "Key": "instock",
	          "Value": "true"
	        },
	        {
	          "Key": "instock_quantity",
	          "Value": "25"
	        }
	      ],
	      "Id": 2,
	      "Description": "",
	      "Location": {
	        "Longitude": -75.953134,
	        "Latitude": 40.801235
	      },
	      "Name": "XOMNI Store,
	      "Address": "105 East 106th Street, New York"
	    }
	  ]
	}


# APIs Used #
 - [Catalog/Item/**Fetching a Single Item**](http://dev.xomni.com/v3-0/http-api/public-apis/catalog/item/fetching-a-single-item)
 - [Catalog/Item In-Store Metadata/**Fetching the in-store metadata of an item**](http://dev.xomni.com/v3-0/http-api/public-apis/catalog/item-in-store-metadata/fetching-the-in-store-metadata-of-an-item)


**Note:**  The API version used in this sample is **v3.0.3**

# XOMNI SDK for .NET #


The code repository for the .NET SDK used in this article can be found [here](https://github.com/XomniCloud/xomni-sdk-dotnet).

# XOMNI Developer Portal #

Feel free to jump into our developer portal to look for more APIs and related documentation. 

[Developer Portal: Public APIs for v3](http://dev.xomni.com/v3-0/http-api/public-apis)

#Bing Maps SDK for Windows 8.1 Apps#

This SDK includes controls for apps built using JavaScript, as well as apps built using C#, C++, and Visual Basic, and requires a Bing Maps Key for a Windows Store app.

[Reference Link]( https://visualstudiogallery.msdn.microsoft.com/224eb93a-ebc4-46ba-9be7-90ee777ad9e1)

