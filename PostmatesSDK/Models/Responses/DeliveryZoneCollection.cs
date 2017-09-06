using System.Collections.Generic;

namespace PostmatesSDK.Models.Responses
{
    /// <summary>
    /// DeliveryZoneCollection
    /// </summary>
    public class DeliveryZoneCollection : BaseResponseModel
    {
        /// <summary>
        /// DeliveryZoneCollection Constrouctor 
        /// </summary>
        public DeliveryZoneCollection()
        {
            Kind = ResponseKinds.delivery_zones;
        }
        /// <summary>
        /// list of DeliveryZones
        /// </summary>
        public IEnumerable<DeliveryZone> DeliveryZones { get; set; }
    }
}