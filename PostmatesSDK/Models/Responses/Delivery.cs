using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PostmatesSDK.Models.Responses
{
    /// <summary>
    /// Delivery Object
    /// </summary>
    public class Delivery : BaseResponseModel
    {
        /// <summary>
        /// Delivery Object
        /// </summary>
        public Delivery() {
            Kind = ResponseKinds.delivery;
        }
        /// <summary>
        /// Id
        /// </summary>
        [JsonProperty("id")] public string Id { get; set; }
        /// <summary>
        /// Date Created
        /// </summary>
        [JsonProperty("created")] public DateTime Created { get; set; }
        /// <summary>
        /// Date Updated
        /// </summary>
        [JsonProperty("updated")] public DateTime Updated { get; set; }
        /// <summary>
        /// pending - We've accepted the delivery and will be assigning it to a courier.
        /// pickup - Courier is assigned and is en route to pick up the items.
        /// pickup_complete - Courier has picked up the items.
        /// dropoff - Courier is moving towards the dropoff.
        /// canceled - Items won't be delivered. Deliveries are either canceled by the customer or by our customer service team.
        /// delivered - Items were delivered successfully.
        /// returned - The delivery was canceled and a new job created to return items to sender. (See related_deliveries in delivery object.)
        /// </summary>
        [JsonProperty("status")] public string Status { get; set; }
        /// <summary>
        /// false if the delivery is ongoing, and you can expect additional updates.
        /// </summary>
        [JsonProperty("complete")] public bool Complete { get; set; }
        /// <summary>
        /// Estimated time the courier will arrive at the pickup location.
        /// </summary>
        [JsonProperty("pickup_eta")] public DateTime? PickupEta { get; set; }
        /// <summary>
        /// Estimated time the courier will arrive at the dropoff location.
        /// </summary>
        [JsonProperty("dropoff_eta")] public DateTime? DropoffEta { get; set; }
        /// <summary>
        /// Based on the delivery window from the delivery quote. If the dropoff_eta goes beyond this dropoff_deadline, our customer service team will be notified. We may extend this value to indicate a known problem.
        /// </summary>
        [JsonProperty("dropoff_deadline")] public DateTime DropoffDeadline { get; set; }
        /// <summary>
        /// ID for the Delivery Quote if one was provided when creating this Delivery.
        /// </summary>
        [JsonProperty("quote_id")] public string QuoteId { get; set; }
        /// <summary>
        /// Amount in cents that will be charged if this delivery is created (see "Other Standards » Currency").
        /// </summary>
        [JsonProperty("fee")] public int Fee { get; set; }
        /// <summary>
        /// Currency the "amount" values are in. (see "Other Standards » Currency").
        /// </summary>
        [JsonProperty("currency")] public string Currency { get; set; }
        /// <summary>
        /// reference - Developer provided identifier for the courier to reference when picking up the package.
        /// returned - A free form body describing the package.
        /// </summary>
        [JsonProperty("manifest")] public Manifest Manifest { get; set; }
        /// <summary>
        /// This field identifies who received delivery at dropoff location.
        /// </summary>
        [JsonProperty("dropoff_identifier")] public string DropoffIdentifier { get; set; }
        /// <summary>
        /// Pickup location
        /// </summary>
        [JsonProperty("pickup")] public Location Pickup { get; set; }
        /// <summary>
        /// Dropoff locations
        /// </summary>
        [JsonProperty("dropoff")] public Location Dropoff { get; set; }
        /// <summary>
        /// The courier object is only present when a delivery is in progress.
        /// name - Courier's first name.
        /// rating - Courier's rating on a scale of 1.0 to 5.0.
        /// vehicle_type - The type of vehicle the courier is using. Currently support bicycle, car, van, truck, scooter, motorcycle, and walker.
        /// phone_number - The courier's phone number.
        /// location - A latitude and longitude indicating courier's location.
        /// img_href - A URL to courier's profile image
        /// </summary>
        [JsonProperty("courier")] public Courier Courier { get; set; }
        /// <summary>
        /// A collection describing other jobs that share an association.
        /// id - Unique identifier.
        /// relationship - Indicating the nature of the job identified in related_deliveries. "original" for the forward leg of the trip, and "returned" for the return leg of the trip.
        /// </summary>
        [JsonProperty("related_deliveries")]
        public IEnumerable<RelatedDeliveries> RelatedDeliveries { get; set; }
        /// <summary>
        /// Tip from customer. can be null
        /// </summary>
        [JsonProperty("tip")]
        public int? Tip { get; set; }
    }
}