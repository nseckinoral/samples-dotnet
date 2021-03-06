# Introduction #
This is a sample project showcasing how consumer devices can be used to login into an In-Store device using their mobile phones. We know the frustration of  typing private credentials on a public device so it’s where XOMNI APIs come in. You can use your own mobile phone to login to the public device and enjoy a presence aware In-Store experience without publicly exposing your password. 

# APIs Used  #

- [Utility/ QR Code/ **Generate QR Code**](http://dev.xomni.com/v3-0/http-api/public-apis/utility/qr-code/generate-qr-code)
- [Company/ Device/ **Registering a Device to a Spesific License**](http://dev.xomni.com/v3-0/http-api/public-apis/company/device/registering-a-device-to-a-specific-license)
- [OmniPlay/ Device/ **Subscribing to OmniPlay Device Queue**](http://dev.xomni.com/v3-0/http-api/public-apis/omniplay/device/subscribing-to-omniplay-device-queue)
- [OmniPlay/ Device/ **Fetching PII User OmniTickets**](http://dev.xomni.com/v3-0/http-api/public-apis/omniplay/device/fetching-pii-user-omnitickets-on-omniplay-device-queue)
- [OmniPlay/ OmniTicket/ **Using OmniTicket for a PII**](http://dev.xomni.com/v3-0/http-api/public-apis/omniplay/omniticket/using-omniticket-for-a-pii)
- [PII/ Wishlist/ **Fetching  All Wishlists**](http://dev.xomni.com/v3-0/http-api/public-apis/pii/wishlist/fetching-all-wish-lists)
- [PII/ Wishlist/ **Fetching a Wishlist With a Unique Key**](http://dev.xomni.com/v3-0/http-api/public-apis/pii/wishlist/fetching-a-wish-list-with-a-wish-list-unique-key)

**Note:**  The API version used in this sample is **v3.0**

# XOMNI Public SDK for .NET #

Preview version of XOMNI Public SDK is used.

The code repository for the SDK can be found [here](https://github.com/XomniCloud/xomni-sdk-dotnet-preview).

# XOMNI Developer Portal #

Feel free to jump into our developer portal to look for more APIs and documentation. 

[Developer Portal: Public APIs for v3.0](http://dev.xomni.com/v3-0/http-api/public-apis)

# Architecture Diagram #

![](https://raw.githubusercontent.com/nseckinoral/samples-dotnet/master/omnilogin-v3-0/Diagram.PNG)

# How To #

## Create a New Client Context ##

First step is to create a clientcontext instance with valid credentials and a valid tenant URL.

		ClientContext sampleclientcontext = new ClientContext("UserName","Password","Service URL");


## Generate a QR Code ##

Generating a QR code is pretty simple. **GetAsync** method of **QRCodeClient** takes 2 parameters :

- Module Size: Basicly the QR size. Additional information can be found [here](http://www.qrcode.com/en/howto/cell.html).
- Data: Any data you want to encode. A dummy link is used in this example.

**Note:** A **device id** is used as a **querystring** in the following example. For more information, please refer to "**Subscribing to a Device Queue**".

		var loginURL= "http://www.example.com"
    	return await clientContext.Of<QRCodeClient>().GetAsync(8, string.Format(loginURL + 
			"?deviceId={0}", "Device Id"));

**Important:** **Response** body contains an **array of bytes** which corresponds to a **PNG image**. You will need to do a **"byte array to image"** conversion. We have excluded to conversion details for the sake of simplicity.

## Registering a Device to a Spesific License ##

This API enables a client application to assign a logical device presence to a specific license. Once you register your **device ID**, it can be used through multiple APIs, in this example while **subscribing to device queue**.


		using (var clientContext = new ClientContext("UserName","Password","Service URL"))
		{
          var deviceClient = clientContext.Of<DeviceClient>();
          var registeredDevice = (await deviceClient.PostAsync(new Device()
          {
            DeviceId = "Device Id",
            Description = "Device Description"
          })).Data;
       }

**Important:** DeviceClient is under **Clients/Company** and Device model used in PostAsync method is under **Models/Company**
## Subscribing to a Device Queue ##

This is where the **Login Page** comes in. We get the users' credentials and create a new **[PII](http://en.wikipedia.org/wiki/Personally_identifiable_information) User**, then subscribe the user to the desired device queue.

**Question:** How do we get the "**device ID**" of the public device?  **Answer** is simple, [querystring](http://www.w3schools.com/asp/coll_querystring.asp), the device ID was part of the URL during the "QR Generation" process.

		var deviceId = Request.QueryString["deviceId"];

 Now for the main part of the topic, final code should look like this.

		[HttpPost]
		public async Task<ActionResult>  Index(Models.User user)
        {
			var clientContext = new ClientContext("UserName","Password","Service URL");
			 
			clientcontext.PIIUser = new User()
            	{
					//User model can be found under Models/PII/User

                	Password = user.Password,
                	UserName = user.Username
            	};

            var deviceId = Request.QueryString["deviceId"];
            if(deviceId != null)
            {
                await clientcontext.Of<DeviceClient>().SubscribeToDevice("Device Id");
            }

			//Do something
		}

**Important:** Users' **private information** is **never sent** to the public device (represented by the device ID), a **PII Token** is sent instead.

## Fetching PII User OmniTickets ##

This part is where you start **polling**, which means waiting a user to **subscribe** to your device queue. Following example shows how to do it **without** a timer, **only once**. 



		using (var clientContext = new ClientContext("UserName","Password","Service URL"))
    	{
    		var deviceClient = clientContext.Of<DeviceClient>();
    		var result = await deviceClient.GetIncomingsAsync(deviceId);
    	}

If a PII user subscribes to the queue successfully, API Response should look like this:


		"Data": [{
			"OmniTicket": "Pbda570b0-cf86-4c86-830c-ac9445faf208",
			"PIIDisplayName": "Example Name",}]

**Important:** **DeviceClient** used in the first example is under **Clients/Omniplay**.

## Generating an Omni Session From an Omni Ticket##

In order to get **PII User**'s data, User's **Ticket** should be converted to an **Omni Session**. 

First step is getting the PII User's Omni Ticket. Following example gets the latest ticket assigned to the user from the appropriate API response.
			
		var latestOmniTicket = result.Data.Last();

Second step is to get rid of the "P" (an identifier for "PII") at the beginning of PII's Ticket.

		var omniTicketString = latestOmniTicket.OmniTicket.Substring(1, latestOmniTicket.OmniTicket.Length - 1);

Last step is to create an instance of OmniTicketClient and generate a session by using **PostSessionAsync** method. This method takes **OmniTicket** object as a **parameter** and **returns OmniSession** object.

		var omniTicketClient = clientContext.Of<XOMNI.SDK.Public.Clients.OmniPlay.OmniTicketClient>();
		omniSession = await omniTicketClient.PostSessionAsync(new OmniTicket 
			{ Ticket = Guid.Parse(omniTicketString) });

**Note:** [Guid.Parse](https://msdn.microsoft.com/en-us/library/system.guid.parse%28v=vs.110%29.aspx) method is used for converting a string to a GUID  	

Your final code will look like this:

		using (var clientContext = new ClientContext("UserName","Password","Service URL"))
    	{
    		var deviceClient = clientContext.Of<DeviceClient>();
    		var result = await deviceClient.GetIncomingsAsync(deviceId);

			var latestOmniTicket = result.Data.Last();
			var omniTicketString = latestOmniTicket.OmniTicket.Substring(1, 
				latestOmniTicket.OmniTicket.Length - 1);
			var omniTicketClient = clientContext.Of<XOMNI.SDK.Public.Clients.OmniPlay.OmniTicketClient>();
			omniSession = await omniTicketClient.PostSessionAsync(new OmniTicket { 
				Ticket = Guid.Parse(omniTicketString) });
    	}

	
## Fetching All Wishlists ##

Users might have more than one wishlist stored in the cloud. Before reaching a spesific wishlist, its better to see the whole image first.

After creating an instance of the **WishlistClient**, simply use the **GetAsync()** method. This method has a "**List of GUID**" return type.

		using (var clientContext = new ClientContext("UserName","Password","Service URL"))
		{
           clientContext.OmniSession = omniSession.Data;

           var wishlistClient = clientContext.Of<WishlistClient>();

           var wishlistGuids = await wishlistClient.GetAsync();
		}

## Fetching a Wishlist With a Unique Key ##

Time to zoom-in into the image and get the full list of products in a particular wishlist. 

Pick a **wishlist** from the "List of Wishlist GUID" first. The following example gets the last one instead of picking a unique ID.

		var latestWishlist = wishlistGuids.Data.Last();

Fetch the items in the wishlist by using **GetAsync** method and you can reach to any detail associated with each item in the list.

		var latestWishlistItems = await wishlistClient.GetAsync(latestWishlist, 11, 12, true, false, false,
			AssetDetailType.None, AssetDetailType.None, AssetDetailType.None, "examplemetadata", 
			"examplemetadata");

**Important:** **AssetDetailType** model is under **Models/Catalog** and **None** corresponds to value **0**.

**Method Parameters In Order:**


1. **wishlistUniqueKey** (String)
1. **longitude** (Number)
1. **latitude** (Number)
1. **includeItemStaticProperties** (Boolean)
1. **includeItemDynamicProperties** (Boolean)
1. **includeCategoryMetadata** (Boolean)
1. **imageAssetDetail** (Number)
1. **videoAssetDetail** (Number)
1. **documentAssetDetail** (Number)
1. **metaDataKey** (String)
1. **metaDataValue** (String)


**Possible Values for Asset Details:**

- **0** Default Value- Image assets are not included in response.
- **1** Includes only default image asset.
- **2** Includes only default video asset with metadata.
- **4** Includes all video assets.
- **8** Includes all video assets with metadata.

Final code will look like below:

		using (var clientContext = new ClientContext("UserName","Password","Service URL"))
		{
           clientContext.OmniSession = omniSession.Data;

           var wishlistClient = clientContext.Of<WishlistClient>();

           var wishlistGuids = await wishlistClient.GetAsync();

		   var latestWishlist = wishlistGuids.Data.Last();

		   var latestWishlistItems = await wishlistClient.GetAsync(latestWishlist, 11, 12, true, false, false,
				AssetDetailType.None, AssetDetailType.None, AssetDetailType.None, "examplemetadata", 
				"examplemetadata");
		}


**Note:** Detailed information about **Parameters** and the **API Response** can be found [**here**](http://dev.xomni.com/v3-0/http-api/public-apis/pii/wishlist/fetching-a-wish-list-with-a-wish-list-unique-key).

Thanks for reading. Let us know what you think!