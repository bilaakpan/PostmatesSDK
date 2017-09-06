using Newtonsoft.Json;

namespace PostmatesSDK.Models
{
    /// <summary>
    /// Courier
    /// </summary>
    public class Courier
    {
        /// <summary>
        /// Name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// Rating
        /// </summary>
        [JsonProperty("rating")]
        public string Rating { get; set; }
        /// <summary>
        /// VehicleType
        /// </summary>
        [JsonProperty("vehicle_type")]
        public string VehicleType { get; set; }
        /// <summary>
        /// PhoneNumber
        /// </summary>
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Coordinate
        /// </summary>
        [JsonProperty("location")]
        public Coordinate Coordinate { get; set; }
        /// <summary>
        /// ImgHref
        /// </summary>
        [JsonProperty("img_href")]
        public string ImgHref { get; set; }
    }
}