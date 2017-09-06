using Newtonsoft.Json;

namespace PostmatesSDK.Models
{
    /// <summary>
    /// DetailedAddress
    /// </summary>
    public class DetailedAddress
    {
        /// <summary>
        /// StreetAddress1
        /// </summary>
        [JsonProperty("street_address_1")]
        public string StreetAddress1 { get; set; }
        /// <summary>
        /// StreetAddress2
        /// </summary>
        [JsonProperty("street_address_2")]
        public string StreetAddress2 { get; set; }
        /// <summary>
        /// City
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }
        /// <summary>
        /// State
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }
        /// <summary>
        /// ZipCode
        /// </summary>
        [JsonProperty("zip_code")]
        public string ZipCode { get; set; }
        /// <summary>
        /// Country
        /// </summary>
        [JsonProperty("country")]
        public string Country { get; set; }
    }
}