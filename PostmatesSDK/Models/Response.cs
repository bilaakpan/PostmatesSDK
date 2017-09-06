using Newtonsoft.Json;
using System;

namespace PostmatesSDK.Models
{
    /// <summary>
    /// Response
    /// </summary>
    public class Response : IResponse
    {
        /// <summary>
        /// Kind
        /// </summary>
        [JsonProperty("kind")]
        public ResponseKinds Kind { get; internal set; }
        internal BaseResponseModel _response { get; set; }
        /// <summary>
        /// Will return the response type based on the Kind preperty 
        /// </summary>
        /// <typeparam name="T">The Response tyep to be returned</typeparam>
        /// <returns>The type of T. if the type of T does not match the Kind an ArgumentException is thrown</returns>
        public T GetResponseObject<T>() where T : BaseResponseModel
        {
            if (_response?.Kind != Kind) {
                throw new ArgumentException("The response kind does not match expected return type");
            }
            return (T)_response;
        }
    }
}
