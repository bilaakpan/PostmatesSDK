using Newtonsoft.Json;
using System;

namespace PostmatesSDK.Models.Responses
{
    /// <summary>
    /// WebHookEvent
    /// </summary>
    public class WebHookEvent: BaseResponseModel
    {
        /// <summary>
        /// Kind
        /// </summary>
        public override ResponseKinds Kind
        {
            get {

                switch (WebHookKind) {
                    case "event.courier_update":
                        return ResponseKinds.event_courier_update;
                    case "event.delivery_deadline":
                        return ResponseKinds.event_delivery_deadline;
                    case "event.delivery_return":
                        return ResponseKinds.event_delivery_return;
                    case "event.delivery_status":
                        return ResponseKinds.event_delivery_status;
                    default:
                        return ResponseKinds.error;
                }
            }            
        }
        [JsonProperty("kind")]
        internal string WebHookKind { get; set; }
        /// <summary>
        /// Event Id
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
        /// <summary>
        /// delivery_id
        /// </summary>
        [JsonProperty("delivery_id")]
        public string DeliveryId { get; set; }
        /// <summary>
        /// delivery status
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
        /// <summary>
        /// The deliver the event was for
        /// </summary>
        [JsonProperty("data")]
        public Delivery Data { get; set; }
        /// <summary>
        /// Date created
        /// </summary>
        [JsonProperty("created")]
        public DateTime Created { get; set; }
    }
}
