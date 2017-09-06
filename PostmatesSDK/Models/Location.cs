using Newtonsoft.Json;

namespace PostmatesSDK.Models
{
    /// <summary>
    /// Location
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// PhoneNumber
        /// </summary>
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Adress
        /// </summary>
        [JsonProperty("address")]
        public string Address { get; set; }
        /// <summary>
        /// DetailedAddress
        /// </summary>
        [JsonProperty("detailed_address")]
        public DetailedAddress DetailedAddress { get; set; }
        /// <summary>
        /// Notes
        /// </summary>
        [JsonProperty("notes")]
        public string Notes { get; set; }
        /// <summary>
        /// Coordinates
        /// </summary>
        [JsonProperty("location")]
        public Coordinate Coordinates { get; set; }
    }
}