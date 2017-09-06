using Newtonsoft.Json;

namespace PostmatesSDK.Models
{
    /// <summary>
    /// RelatedDeliveries
    /// </summary>
    public class RelatedDeliveries
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
        /// <summary>
        /// Relationship
        /// </summary>
        [JsonProperty("relationship")]
        public string Relationship { get; set; }
    }
}