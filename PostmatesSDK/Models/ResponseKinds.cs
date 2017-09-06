namespace PostmatesSDK.Models
{
    /// <summary>
    /// Postmates Response Types
    /// </summary>
    public enum ResponseKinds
    {
        /// <summary>
        /// error
        /// </summary>
        error = 0,
        /// <summary>
        /// delivery_quote
        /// </summary>
        delivery_quote = 1,
        /// <summary>
        /// delivery_zones
        /// </summary>
        delivery_zones = 2,
        /// <summary>
        /// delivery
        /// </summary>
        delivery = 3,
        /// <summary>
        /// delivery_list
        /// </summary>
        delivery_list = 4,
        /// <summary>
        /// event_delivery_status
        /// </summary>
        event_delivery_status = 5,
        /// <summary>
        /// event_delivery_deadline
        /// </summary>
        event_delivery_deadline = 6,
        /// <summary>
        /// event_courier_update
        /// </summary>
        event_courier_update = 7,
        /// <summary>
        /// event_delivery_return
        /// </summary>
        event_delivery_return = 8
    }
}