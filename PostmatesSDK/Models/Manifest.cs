using Newtonsoft.Json;

namespace PostmatesSDK.Models
{
    /// <summary>
    /// Manifest
    /// </summary>
    public class Manifest
    {
        /// <summary>
        /// Reference
        /// </summary>
        [JsonProperty("reference")]
        public string Reference { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}