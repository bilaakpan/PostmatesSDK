# PostmatesSDK
Postmates .NET SDK

Nuget can be found here https://www.nuget.org/packages/PostmatesSDK/



Features
--------
Postmates is a [NuGet library](https://www.nuget.org/packages/PostmatesSDK) that you can add in to your project to make use of the Postmastes delivery service.


Hot to use
------------------------------------------------------------
Start by making a PostmatesClient object.
(*note that this client can be static and and used like a singlton but should be disposed to release network resources*)
```csharp
var client = new PostmatesClient("customer_id", "username")
```
The PostmatesClient implements IPostmatesClient that has these members

```csharp
Task<IResponse> AddTipAsync(string deliveryId, int tip);
Task<IResponse> CancelDeliveryAsync(string deliveryId);
Task<IResponse> CreateDeliveryAsync(CreateDelivery createDelivery);
Task<IResponse> GetDeliveryAsync(string deliveryId);
Task<IResponse> GetDeliveryListAsync(string filter = "ongoing", string next_href = null);
Task<IResponse> GetDeliveryZonesAsync();
Task<IResponse> GetQuoteAsync(Address pickupAddress, Address dropoffAddress);
```
All calls return the same type of IResponse that has two members
```csharp
ResponseKinds Kind { get; }
T GetResponseObject<T>() where T : BaseResponseModel;
```

in the case of an error the Kind property of the response will be ResponseKinds.error below are all kinds

```csharp
error = 0, // Expect type of ErrorResponse
delivery_quote = 1,// Expect type of DeliveryQuote
delivery_zones = 2,// Expect type of DeliveryZoneCollection
delivery = 3,// Expect type of Delivery
delivery_list = 4,// Expect type of DeliveryCollection
event_delivery_status = 5,// Expect type of WebHookEvent
event_delivery_deadline = 6,// Expect type of WebHookEvent
event_courier_update = 7,// Expect type of WebHookEvent
event_delivery_return = 8// Expect type of WebHookEvent
```
Example usage:

```csharp
using (var client = new PostmatesClient("customer_id", "username")) {
    var pickupAddress = new Address { StreetNumber = "20 McAllister St", City = "San Francisco", State = "CA" };
    var dropoffAddress = new Address { StreetNumber = "201 Market St", City = "San Francisco", State = "CA" };

    var response = await client.GetQuoteAsync(pickupAddress, dropoffAddress);

    if (response.Kind == ResponseKinds.delivery_quote) {
        var quote = response.GetResponseObject<DeliveryQuote>();
        // Do biz logic
    }
    else {
        var err = response.GetResponseObject<ErrorResponse>();
        // Do biz logic and log error
        }
    }
```
