using Newtonsoft.Json;
using System;

namespace PostmatesSDK.Models.Responses
{
    /// <summary>
    /// DeliveryQuote
    /// </summary>
    public class DeliveryQuote : BaseResponseModel
    {
        /// <summary>
        /// DeliveryQuote Constructor
        /// </summary>
        public DeliveryQuote()
        {
            Kind = ResponseKinds.delivery_quote;
        }
        /// <summary>
        /// QuoteId
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
        /// <summary>
        /// Date Created
        /// </summary>
        [JsonProperty("created")]
        public DateTime Created { get; set; }
        /// <summary>
        /// Date/Time after which the quote will no longer be accepted.
        /// </summary>
        [JsonProperty("expires")]
        public DateTime Expires { get; set; }
        /// <summary>
        /// Amount in cents that will be charged if this delivery is created (see "Other Standards » Currency").
        /// </summary>
        [JsonProperty("fee")]
        public int Fee { get; set; }
        /// <summary>
        /// Currency the "amount" values are in. (see "Other Standards » Currency").
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }
        /// <summary>
        /// Estimated drop-off time. This value may increase to several hours if the postmates platform is in high demand.
        /// </summary>
        [JsonProperty("dropoff_eta")]
        public DateTime DropoffEta { get; set; }
        /// <summary>
        /// Estimated minutes for this delivery to reach dropoff, this value can be highly variable based upon the current amount of capacity available.
        /// </summary>
        [JsonProperty("duration")]
        public int Duration { get; set; }
    }
}