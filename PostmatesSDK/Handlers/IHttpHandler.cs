using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PostmatesSDK.Handlers
{
    /// <summary>
    /// IHttpHandler
    /// </summary>
    public interface IHttpHandler : IDisposable
    {
        /// <summary>
        /// GetAsync
        /// </summary>
        /// <param name="url"></param>
        /// <returns>Task of type HttpResponseMessage</returns>
        Task<HttpResponseMessage> GetAsync(string url);
        /// <summary>
        /// PostAsync
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
       /// <returns>Task of type HttpResponseMessage</returns>
        Task<HttpResponseMessage> PostAsync(string url, HttpContent content);
        /// <summary>
        /// PutAsync
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
       /// <returns>Task of type HttpResponseMessage</returns>
        Task<HttpResponseMessage> PutAsync(string url, HttpContent content);
        /// <summary>
        /// DeleteAsync
        /// </summary>
        /// <param name="url"></param>
       /// <returns>Task of type HttpResponseMessage</returns>
        Task<HttpResponseMessage> DeleteAsync(string url);
    }
}
