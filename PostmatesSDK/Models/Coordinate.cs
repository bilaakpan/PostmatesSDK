using Newtonsoft.Json;

namespace PostmatesSDK.Models
{
    /// <summary>
    /// Coordinate
    /// </summary>
    public class Coordinate
    {
        /// <summary>
        /// Lat
        /// </summary>
        [JsonProperty("lat")]
        public float Lat { get; set; }
        /// <summary>
        /// Lng
        /// </summary>
        [JsonProperty("lng")]
        public float Lng { get; set; }
    }
}