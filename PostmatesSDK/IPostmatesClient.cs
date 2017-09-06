using System.Threading.Tasks;
using PostmatesSDK.Models;
using System;

namespace PostmatesSDK
{
    /// <summary>
    /// IPostmatesClient
    /// </summary>
    public interface IPostmatesClient : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deliveryId"></param>
        /// <param name="tip"></param>
        /// <returns></returns>
        Task<IResponse> AddTipAsync(string deliveryId, int tip);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deliveryId"></param>
        /// <returns></returns>
        Task<IResponse> CancelDeliveryAsync(string deliveryId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="createDelivery"></param>
        /// <returns></returns>
        Task<IResponse> CreateDeliveryAsync(CreateDelivery createDelivery);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deliveryId"></param>
        /// <returns></returns>
        Task<IResponse> GetDeliveryAsync(string deliveryId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="next_href"></param>
        /// <returns></returns>
        Task<IResponse> GetDeliveryListAsync(string filter = "ongoing", string next_href = null);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IResponse> GetDeliveryZonesAsync();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pickupAddress"></param>
        /// <param name="dropoffAddress"></param>
        /// <returns></returns>
        Task<IResponse> GetQuoteAsync(Address pickupAddress, Address dropoffAddress);
    }
}