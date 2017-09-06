using GeoJSON.Net.Feature;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace PostmatesSDK.Models
{
    /// <summary>
    /// DeliveryZone
    /// </summary>
    public class DeliveryZone
    {
        /// <summary>
        /// Propertie
        /// </summary>
        public class Propertie
        {
            /// <summary>
            /// ZoneName
            /// </summary>
            [JsonProperty("zone_name")]
            public string ZoneName { get; set; }
            /// <summary>
            /// MarketName
            /// </summary>
            [JsonProperty("market_name")]
            public string MarketName { get; set; }
        }
        /// <summary>
        /// Properties
        /// </summary>
        [JsonProperty("properties")]
        public Propertie Properties { get; set; }
        /// <summary>
        /// Features
        /// </summary>
        [JsonProperty("features")]
        public IEnumerable<Feature> Features { get; set; }
    }
}