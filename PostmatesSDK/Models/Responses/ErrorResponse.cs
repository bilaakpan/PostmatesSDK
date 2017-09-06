using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PostmatesSDK.Models.Responses
{
    /// <summary>
/// ErrorResponse
/// </summary>
    public class ErrorResponse : BaseResponseModel
    {
        /// <summary>
        /// ErrorResponse Constructor
        /// </summary>
        public ErrorResponse()
        {
            Kind = ResponseKinds.error;
        }
        /// <summary>
        /// Code
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
        /// <summary>
        /// HttpStatusCodes
        /// </summary>
        public int HttpStatusCodes { get; set; }
        /// <summary>
        /// Params
        /// </summary>
        [JsonProperty("params")]
        public IDictionary<string,string> Params { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Exception Exception { get; set; }
    }
}
