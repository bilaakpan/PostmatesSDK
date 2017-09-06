using Newtonsoft.Json;

namespace PostmatesSDK.Models
{
    /// <summary>
    /// BaseResponseModel
    /// </summary>
    public class BaseResponseModel
    {
        //[JsonProperty("kind")]
        /// <summary>
        /// Kind
        /// </summary>
        public virtual ResponseKinds Kind { get; internal set; }
        /// <summary>
        /// LiveMode
        /// </summary>
        [JsonProperty("live_mode")]
        public bool LiveMode { get; internal set; }        
    }
}
