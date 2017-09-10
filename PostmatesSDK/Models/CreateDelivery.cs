
namespace PostmatesSDK.Models
{
    /// <summary>
    /// CreateDelivery
    /// </summary>
    public class CreateDelivery
    {
        /// <summary>
        /// The ID of a previously generated delivery quote. Optional, but recommended. Example: "del_KSsT9zJdfV3P9k"
        /// </summary>
        public string QuoteId { get; set; }
        /// <summary>
        /// A detailed description of what the courier will be delivering. Example: "A box of gray kittens"
        /// </summary>
        public string Manifest { get; set; }
        /// <summary>
        /// Optional reference that identifies the manifest. Example: "Order #690"
        /// </summary>
        public string ManifestReference { get; set; }

        /// <summary>
        /// Name of the place where the courier will make the pickup. Example: "Kitten Warehouse"
        /// </summary>
        public string PickupName { get; set; }

        /// <summary>
        /// The pickup address for the delivery. Example: "20 McAllister St, San Francisco, CA"
        /// </summary>
        public Address PickupAddress { get; set; }

        /// <summary>
        /// The phone number of the pickup location. Example: "415-555-4242"
        /// </summary>
        public string PickupPhoneNumber { get; set; }

        /// <summary>
        /// Optional business name of the pickup location. Example: "Feline Enterprises, Inc."
        /// </summary>
        public string PickupBusinessName { get; set; }

        /// <summary>
        /// Additional instructions for the courier at the pickup location. Example: "Ring the doorbell twice, and only delivery the package if a human answers."
        /// </summary>
        public string PickupNotes { get; set; }

        /// <summary>
        /// Name of the place where the courier will make the dropoff. Example: "Alice"
        /// </summary>
        public string DropoffName { get; set; }

        /// <summary>
        /// The dropoff address for the delivery. Example: "678 Green St, San Francisco, CA"
        /// </summary>
        public Address DropoffAddress { get; set; }

        /// <summary>
        /// The phone number of the dropoff location. Example: "415-555-8484"
        /// </summary>
        public string DropoffPhoneNumber { get; set; }
        /// <summary>
        /// Optional business name of the dropoff location. Example: "Alice's Cat Cafe"
        /// </summary>
        public string DropoffBusinessName { get; set; }
        /// <summary>
        /// Additional instructions for the courier at the dropoff location. Example: "Tell the security guard that you're here to see Alice."
        /// </summary>
        public string DropoffNotes { get; set; }

        internal void Validate()
        {
            ThrowIfNullOrEmpty(PickupName, nameof(PickupName));
            ThrowIfNullOrEmpty(DropoffName, nameof(DropoffName));
            ThrowIfNullOrEmpty(Manifest, nameof(Manifest));
            ThrowIfNullOrEmpty(PickupPhoneNumber, nameof(PickupPhoneNumber));
            ThrowIfNullOrEmpty(DropoffPhoneNumber, nameof(DropoffPhoneNumber));
            ThrowIfNullOrEmpty(DropoffAddress, nameof(DropoffAddress));
            ThrowIfNullOrEmpty(PickupAddress, nameof(PickupAddress));
        }
        private void ThrowIfNullOrEmpty(string value, string propertyName)
        {
            if (string.IsNullOrEmpty(value) ){
                throw new System.ArgumentNullException(propertyName);
            }
        }
        private void ThrowIfNullOrEmpty(object value, string propertyName)
        {
            if (value==null) {
                throw new System.ArgumentNullException(propertyName);
            }
        }
    }
}