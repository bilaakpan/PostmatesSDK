using PostmatesSDK.Handlers;
using PostmatesSDK.Models;
using PostmatesSDK.Models.Responses;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
namespace PostmatesSDK
{
    /// <summary>
    /// PostmatesClient
    /// </summary>
    public class PostmatesClient : IPostmatesClient
    {
        private string _customer_Id;
        private string _username;
        private IHttpHandler _httpClient;
        /// <summary>
        /// PostmatesBaseUrl
        /// </summary>
        public Uri PostmatesBaseUrl = new Uri("https://api.postmates.com/");

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="customer_id"></param>
        /// <param name="username"></param>
        public PostmatesClient(string customer_id, string username)
        {
            Inet(customer_id, username, new HttpHandler(PostmatesBaseUrl, username));
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="customer_id"></param>
        /// <param name="username"></param>
        /// <param name="httpHandler"></param>
        public PostmatesClient(string customer_id, string username, IHttpHandler httpHandler)
        {
            Inet(customer_id, username, httpHandler);
        }
        private void Inet(string customer_id, string username, IHttpHandler httpHandler)
        {
            if (string.IsNullOrEmpty(customer_id)) {
                throw new ArgumentException("cant be null", nameof(customer_id));
            }

            if (string.IsNullOrEmpty(username)) {
                throw new ArgumentException("cant be null", nameof(username));
            }

            _customer_Id = customer_id;
            _username = username;
            _httpClient = httpHandler;
        }
        /// <summary>
        /// A Delivery Quote provides an estimate for a potential delivery. This includes the amount the delivery is expected to cost as well as an estimated delivery window. As demand on our platform changes over time, the fee amount and ETA may increase beyond what your use-case can support.
        ///  A Delivery Quote can only be used once and is only valid for a limited duration.
        /// </summary>
        /// <param name="pickupAddress">The pickup address for a potential delivery.</param>
        /// <param name="dropoffAddress">The dropoff address for a potential delivery.</param>
        /// <returns>if sucessful DeliveryQuote else error</returns>
        public async Task<IResponse> GetQuoteAsync(Address pickupAddress, Address dropoffAddress)
        {
            if (pickupAddress == null) {
                throw new ArgumentNullException(nameof(pickupAddress));
            }

            if (dropoffAddress == null) {
                throw new ArgumentNullException(nameof(dropoffAddress));
            }

            using (var quoteResponse = await _httpClient.PostAsync($"/v1/customers/{_customer_Id}/delivery_quotes",
                new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string, string>("pickup_address", $"{pickupAddress.StreetNumber},{pickupAddress.City},{pickupAddress.State}"),
                    new KeyValuePair<string, string>("dropoff_address", $"{dropoffAddress.StreetNumber},{dropoffAddress.City},{dropoffAddress.State}")
                })).ConfigureAwait(false)) {

                return await GetResponseAsync<DeliveryQuote>(quoteResponse, ResponseKinds.delivery_quote).ConfigureAwait(false);
            }
        }
        /// <summary>
        /// Coordinates will be in the format [longitude, latitude].
        /// Our zones are not bound by zip code borders.If you need to check to see if an address is within a given zone, use the Delivery Quote
        /// </summary>
        /// <returns>if sucessful DeliveryZoneCollection else error</returns>
        public async Task<IResponse> GetDeliveryZonesAsync()
        {
            var response = new Response();
            var contentString = "";

            using (var zoneResponse = await _httpClient.GetAsync("/v1/delivery_zones").ConfigureAwait(false)) {

                try {

                    contentString = await zoneResponse?.Content.ReadAsStringAsync();
                    zoneResponse.EnsureSuccessStatusCode();
                    response._response = new DeliveryZoneCollection {
                        DeliveryZones =
                        Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<DeliveryZone>>(contentString)
                    }; ;
                    response.Kind = ResponseKinds.delivery_zones;
                }
                catch (Exception ex) {
                    response._response = GetErrorResponse(contentString, zoneResponse.StatusCode, ex);
                    response.Kind = ResponseKinds.error;
                }
            }
            return response;
        }
        /// <summary>
        /// After you've successfully created a delivery quote, it's time to create an actual delivery on the Postmates platform. It's recommended that you include the previously generated quote_id to ensure the costs and ETAs are consistent with the quote. Once a delivery is accepted, the delivery fee will be deducted from your account.
        /// </summary>
        /// <param name="createDelivery"></param>
        /// <returns>if sucessful DeliveryQuote else error</returns>
        public async Task<IResponse> CreateDeliveryAsync(CreateDelivery createDelivery)
        {
            if (createDelivery == null) {
                throw new ArgumentNullException(nameof(createDelivery));
            }

            createDelivery.Validate();

            using (var deliveryResponse = await _httpClient.PostAsync($"/v1/customers/{_customer_Id}/deliveries",
               new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string, string>("pickup_address", $"{createDelivery.PickupAddress.StreetNumber},{createDelivery.PickupAddress.City},{createDelivery.PickupAddress.State}"),
                    new KeyValuePair<string, string>("dropoff_address", $"{createDelivery.DropoffAddress.StreetNumber},{createDelivery.DropoffAddress.City},{createDelivery.DropoffAddress.State}"),
                    new KeyValuePair<string, string>("quote_id",createDelivery.QuoteId),
                    new KeyValuePair<string, string>("manifest",createDelivery.Manifest),
                    new KeyValuePair<string, string>("manifest_reference",createDelivery.ManifestReference),
                    new KeyValuePair<string, string>("pickup_name",createDelivery.PickupName),
                    new KeyValuePair<string, string>("pickup_phone_number",createDelivery.PickupPhoneNumber),
                    new KeyValuePair<string, string>("pickup_business_name",createDelivery.PickupBusinessName),
                    new KeyValuePair<string, string>("pickup_notes",createDelivery.PickupNotes),
                    new KeyValuePair<string, string>("dropoff_name",createDelivery.DropoffName),
                    new KeyValuePair<string, string>("dropoff_phone_number",createDelivery.DropoffPhoneNumber),
                    new KeyValuePair<string, string>("dropoff_business_name",createDelivery.DropoffBusinessName),
                    new KeyValuePair<string, string>("dropoff_notes",createDelivery.DropoffNotes)
               })).ConfigureAwait(false)) {

                return await GetResponseAsync<Delivery>(deliveryResponse, ResponseKinds.delivery).ConfigureAwait(false);
            }
        }
        /// <summary>
        /// List all deliveries for a customer.
        /// </summary>
        /// <param name="filter">This filter limits the results to only deliveries that are currently being delivered. pass null for no filter</param>
        /// <param name="next_href">path to next page</param>
        /// <returns></returns>
        public async Task<IResponse> GetDeliveryListAsync(string filter = "ongoing", string next_href = null)
        {
            var uriPath = next_href ?? $"/v1/customers/{_customer_Id}/deliveries?filter=" + filter ?? "";
            using (var deliveryList = await _httpClient.GetAsync(uriPath).ConfigureAwait(false)) {
                return await GetResponseAsync<DeliveryCollection>(deliveryList, ResponseKinds.delivery_list).ConfigureAwait(false);
            }
        }
        /// <summary>
        /// Retrieve updated details about a delivery.
        /// </summary>
        /// <param name="deliveryId"></param>
        /// <returns></returns>
        public async Task<IResponse> GetDeliveryAsync(string deliveryId)
        {
            if (string.IsNullOrEmpty(deliveryId)) {
                throw new ArgumentException("Can't be null or empty", nameof(deliveryId));
            }

            using (var delivery = await _httpClient.GetAsync($"/v1/customers/{_customer_Id}/deliveries/{deliveryId}").ConfigureAwait(false)) {
                return await GetResponseAsync<Delivery>(delivery, ResponseKinds.delivery).ConfigureAwait(false);
            }
        }
        /// <summary>
        /// Cancel an ongoing delivery. A delivery can only be canceled prior to a courier completing pickup. Delivery fees still apply.
        /// </summary>
        /// <param name="deliveryId"></param>
        /// <returns></returns>
        public async Task<IResponse> CancelDeliveryAsync(string deliveryId)
        {
            if (string.IsNullOrEmpty(deliveryId)) {
                throw new ArgumentException("Can't be null or empty", nameof(deliveryId));
            }

            using (var delivery = await _httpClient.GetAsync($"/v1/customers/{_customer_Id}/deliveries/{deliveryId}/cancel").ConfigureAwait(false)) {
                return await GetResponseAsync<Delivery>(delivery, ResponseKinds.delivery).ConfigureAwait(false);
            }
        }
        /// <summary>
        /// After an order has completed, you can add a tip for the courier for up to 7 days.
        /// </summary>
        /// <param name="deliveryId"></param>
        /// <param name="tip">Amount in cents that will be paid to the courier as a tip.</param>
        /// <returns></returns>
        public async Task<IResponse> AddTipAsync(string deliveryId, int tip)
        {
            if (string.IsNullOrEmpty(deliveryId)) {
                throw new ArgumentException("message", nameof(deliveryId));
            }

            if (tip <= 0) {
                throw new ArgumentOutOfRangeException(nameof(tip), tip, "must be grater than 0.");
            }

            using (var quoteResponse = await _httpClient.PostAsync($"/v1/customers/{_customer_Id}/deliveries/{deliveryId}",
                  new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string, string>("tip_by_customer", tip.ToString())
                  })).ConfigureAwait(false)) {

                return await GetResponseAsync<Delivery>(quoteResponse, ResponseKinds.delivery).ConfigureAwait(false);
            }
        }
        private ErrorResponse GetErrorResponse(string contentString, HttpStatusCode httpStatussCode, Exception exception)
        {
            var errorResponse = new ErrorResponse();

            if (!string.IsNullOrEmpty(contentString)) {
                errorResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ErrorResponse>(contentString);
            }
            errorResponse.HttpStatusCodes = (int)httpStatussCode;
            errorResponse.Exception = exception;
           
            return errorResponse;
        }
        private async Task<IResponse> GetResponseAsync<T>(HttpResponseMessage responseMessage, ResponseKinds kind) where T : BaseResponseModel
        {
            var response = new Response();
            var contentString = "";
            try {
                contentString = await responseMessage?.Content.ReadAsStringAsync();
                responseMessage.EnsureSuccessStatusCode();
                response._response = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(contentString);
                response.Kind = kind;
            }
            catch (Exception ex) {
                response._response = GetErrorResponse(contentString, responseMessage.StatusCode, ex);
                response.Kind = ResponseKinds.error;
            }

            return response;
        }
        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (disposing) {
                _httpClient.Dispose();
            };

        }
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Detructor
        /// </summary>
        ~PostmatesClient()
        {
            Dispose(false);
        }       
    }
}