using Newtonsoft.Json;
using System.Collections.Generic;

namespace PostmatesSDK.Models.Responses
{
    /// <summary>
    /// DeliveryCollection
    /// </summary>
    public class DeliveryCollection : BaseResponseModel
    {
        /// <summary>
        /// DeliveryCollection constructor
        /// </summary>
        public DeliveryCollection()
        {
            Kind = ResponseKinds.delivery_list;
        }
        /// <summary>
        /// TotalCount
        /// </summary>
        [JsonProperty("total_count")]
        public int TotalCount { get; set; }
        /// <summary>
        /// NextHref
        /// </summary>
        [JsonProperty("next_href")]
        public string NextHref { get; set; }
        /// <summary>
        /// Deliveries
        /// </summary>
        [JsonProperty("data")]
        public IEnumerable<Delivery> Deliveries { get; set; }
    }
}