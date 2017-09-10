using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostmatesSDK;
using PostmatesSDK.Handlers;
using PostmatesSDK.Models;
using PostmatesSDK.Models.Responses;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SDKUnitTest
{

    [TestClass]

    public class UnitTest
    {
        [TestMethod]
        public void CLIENT_CREATION() => new PostmatesClient("clientID", "Username");
        [TestMethod]
        public void CLIENT_CREATION_REQUIRES_CUSTOMERID() =>
             Assert.ThrowsException<ArgumentException>(() => new PostmatesClient("", "username"));
        [TestMethod]
        public void CLIENT_CREATION_REQUIRES_USERNAME() =>
           Assert.ThrowsException<ArgumentException>(() => new PostmatesClient("clientID", ""));

        [TestMethod]
        public void REQUEST_INVALID_KIND_TYPE_FOR_RESPONSE()
        => Assert.ThrowsException<ArgumentException>(() => new Response().GetResponseObject<DeliveryQuote>());

        [TestMethod]
        public void GET_A_DELIVERY_QUOTE()
        {
            var handler = A.Fake<IHttpHandler>();
            var quoteString = "{\"kind\": \"delivery_quote\",\"id\": \"dqt_qUdje83jhdk\",\"created\": \"2014-08-26T10:04:03Z\",\"expires\": \"2014-08-26T10:09:03Z\",\"fee\": 799,\"currency\": \"usd\",\"dropoff_eta\": \"2014-08-26T12:15:03Z\",\"duration\": 60}";

            var httpResponse = new HttpResponseMessage {
                Content = new StringContent(quoteString),
                StatusCode = System.Net.HttpStatusCode.OK
            };
            var pickupAddress = new Address { StreetNumber = "20 McAllister St", City = "San Francisco", State = "CA" };
            var dropoffAddress = new Address { StreetNumber = "201 Market St", City = "San Francisco", State = "CA" };

            A.CallTo(handler).WithReturnType<Task<HttpResponseMessage>>().WithAnyArguments().Returns(Task.FromResult(httpResponse));

            var client = new PostmatesClient("clientID", "username", handler);

            var response = client.GetQuoteAsync(pickupAddress, dropoffAddress).Result;

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseKinds.delivery_quote, response.Kind);
            Assert.IsTrue(response.GetResponseObject<DeliveryQuote>() is DeliveryQuote);
            var quote = response.GetResponseObject<DeliveryQuote>();
            Assert.IsTrue(!string.IsNullOrEmpty(quote.Id));
            Assert.IsTrue(quote.Created != new DateTime());
            Assert.IsTrue(quote.Expires != new DateTime());
            Assert.IsTrue(quote.DropoffEta != new DateTime());
            Assert.AreEqual("usd", quote.Currency);
            Assert.IsTrue(quote.Duration > 0);
            Assert.IsTrue(quote.Fee > 0);
        }
        [TestMethod]
        public void GET_ERROR_A_DELIVERY_QUOTE()
        {
            var handler = A.Fake<IHttpHandler>();
            var errorString = "{\"kind\": \"error\",\"code\": \"invalid_params\",\"message\": \"The parameters of your request were invalid.\"}";

            var httpResponse = new HttpResponseMessage {
                Content = new StringContent(errorString),
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
            var pickupAddress = new Address { StreetNumber = "20 McAllister St", City = "San Francisco", State = "CA" };
            var dropoffAddress = new Address { StreetNumber = "201 Market St", City = "San Francisco", State = "CA" };

            A.CallTo(handler).WithReturnType<Task<HttpResponseMessage>>().WithAnyArguments().Returns(Task.FromResult(httpResponse));

            var client = new PostmatesClient("clientID", "username", handler);

            var response = client.GetQuoteAsync(pickupAddress, dropoffAddress).Result;

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseKinds.error, response.Kind);
            Assert.IsTrue(response.GetResponseObject<ErrorResponse>() is ErrorResponse);
            var error = response.GetResponseObject<ErrorResponse>();
            Assert.IsTrue(!string.IsNullOrEmpty(error.Code));
            Assert.IsTrue(!string.IsNullOrEmpty(error.Message));
            Assert.AreEqual((int)System.Net.HttpStatusCode.NotFound, error.HttpStatusCodes);
        }
        [TestMethod]
        public void GET_A_DELIVERY_QUOTE_VAILD_PICKUPADDRESS()
        => Assert.ThrowsException<AggregateException>(() => new PostmatesClient("customer_Id", "username").GetQuoteAsync(null, new Address()).Result);

        [TestMethod]
        public void GET_A_DELIVERY_QUOTE_VAILD_DROPOFFADDRESS()
         => Assert.ThrowsException<AggregateException>(() => new PostmatesClient("customer_Id", "username").GetQuoteAsync(new Address(), null).Result);

        [TestMethod]
        [DeploymentItem("GET_DELIVERY_ZONES_RESPONSE.json", "targetFolder")]
        public void GET_DELIVERY_ZONES()
        {
            var handler = A.Fake<IHttpHandler>();
            var responseData = System.IO.File.ReadAllText(@"targetFolder\GET_DELIVERY_ZONES_RESPONSE.json");

            var httpResponse = new HttpResponseMessage {
                Content = new StringContent(responseData),
                StatusCode = System.Net.HttpStatusCode.OK
            };

            A.CallTo(handler).WithReturnType<Task<HttpResponseMessage>>().WithAnyArguments().Returns(Task.FromResult(httpResponse));

            var client = new PostmatesClient("clientID", "username", handler);

            var response = client.GetDeliveryZonesAsync().Result;

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseKinds.delivery_zones, response.Kind);
            Assert.IsTrue(response.GetResponseObject<DeliveryZoneCollection>() is DeliveryZoneCollection);
            var zones = response.GetResponseObject<DeliveryZoneCollection>();
            Assert.IsNotNull(zones);
            Assert.IsNotNull(zones.DeliveryZones);
            Assert.IsTrue(zones.DeliveryZones.Any());
        }

        [TestMethod]
        public void GET_ERROR_DELIVERY_ZONES()
        {
            var handler = A.Fake<IHttpHandler>();
            var errorString = "{\"kind\": \"error\",\"code\": \"invalid_params\",\"message\": \"The parameters of your request were invalid.\"}";

            var httpResponse = new HttpResponseMessage {
                Content = new StringContent(errorString),
                StatusCode = System.Net.HttpStatusCode.BadRequest
            };

            A.CallTo(handler).WithReturnType<Task<HttpResponseMessage>>().WithAnyArguments().Returns(Task.FromResult(httpResponse));

            var client = new PostmatesClient("clientID", "username", handler);

            var response = client.GetDeliveryZonesAsync().Result;

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseKinds.error, response.Kind);
            Assert.IsTrue(response.GetResponseObject<ErrorResponse>() is ErrorResponse);
            var error = response.GetResponseObject<ErrorResponse>();
            Assert.IsTrue(!string.IsNullOrEmpty(error.Code));
            Assert.IsTrue(!string.IsNullOrEmpty(error.Message));
            Assert.AreEqual((int)System.Net.HttpStatusCode.BadRequest, error.HttpStatusCodes);
        }
        [TestMethod]
        public void CREATE_A_DELIVERY_MUST_HAVE_NOT_NULL_RQUEST() =>
            Assert.ThrowsException<AggregateException>(() => new PostmatesClient("clientID", "username").CreateDeliveryAsync(null).Result);
        [TestMethod]
        [DeploymentItem("CREATE_A_DELIVERY_RESPONSE.json", "targetFolder")]
        public void CREATE_A_DELIVERY()
        {
            var handler = A.Fake<IHttpHandler>();
            var deliverytring = System.IO.File.ReadAllText(@"targetFolder\CREATE_A_DELIVERY_RESPONSE.json");

            var httpResponse = new HttpResponseMessage {
                Content = new StringContent(deliverytring),
                StatusCode = System.Net.HttpStatusCode.OK
            };

            A.CallTo(handler).WithReturnType<Task<HttpResponseMessage>>().WithAnyArguments().Returns(Task.FromResult(httpResponse));

            var client = new PostmatesClient("clientID", "username", handler);

            var requst = new CreateDelivery {
                QuoteId = "ssdfsgkfgf",
                PickupAddress = new Address { StreetNumber = "20 McAllister St", City = "San Francisco", State = "CA" },
                DropoffAddress = new Address { StreetNumber = "201 Market St", City = "San Francisco", State = "CA" },
                PickupName = "PickUpName",
                DropoffName = "DropoffName",
                Manifest = "Manifest",
                PickupPhoneNumber = "111-111-1111",
                DropoffPhoneNumber = "111-111-1111"
            };

            var response = client.CreateDeliveryAsync(requst).Result;

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseKinds.delivery, response.Kind);
            Assert.IsTrue(response.GetResponseObject<Delivery>() is Delivery);
            var delivery = response.GetResponseObject<Delivery>();
            Assert.IsNotNull(delivery);
        }
        [TestMethod]
        public void CREATE_ERROR_A_DELIVERY()
        {
            var handler = A.Fake<IHttpHandler>();
            var errorString = "{\"kind\": \"error\",\"code\": \"invalid_params\",\"message\": \"The parameters of your request were invalid.\"}";

            var httpResponse = new HttpResponseMessage {
                Content = new StringContent(errorString),
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };

            A.CallTo(handler).WithReturnType<Task<HttpResponseMessage>>().WithAnyArguments().Returns(Task.FromResult(httpResponse));

            var client = new PostmatesClient("clientID", "username", handler);
            var requst = new CreateDelivery {
                PickupAddress = new Address { StreetNumber = "20 McAllister St", City = "San Francisco", State = "CA" },
                DropoffAddress = new Address { StreetNumber = "201 Market St", City = "San Francisco", State = "CA" },
                PickupName = "PickUpName",
                DropoffName = "DropoffName",
                Manifest = "Manifest",
                PickupPhoneNumber = "111-111-1111",
                DropoffPhoneNumber = "111-111-1111"
            };


            var response = client.CreateDeliveryAsync(requst).Result;


            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseKinds.error, response.Kind);
            Assert.IsTrue(response.GetResponseObject<ErrorResponse>() is ErrorResponse);
            var error = response.GetResponseObject<ErrorResponse>();
            Assert.IsTrue(!string.IsNullOrEmpty(error.Code));
            Assert.IsTrue(!string.IsNullOrEmpty(error.Message));
            Assert.AreEqual((int)System.Net.HttpStatusCode.InternalServerError, error.HttpStatusCodes);
        }

        [TestMethod]
        [DeploymentItem("LIST_DELIVERIES_RESPONSE.json", "targetFolder")]
        public void LIST_DELIVERIES()
        {
            var handler = A.Fake<IHttpHandler>();
            var responseData = System.IO.File.ReadAllText(@"targetFolder\LIST_DELIVERIES_RESPONSE.json");

            var httpResponse = new HttpResponseMessage {
                Content = new StringContent(responseData),
                StatusCode = System.Net.HttpStatusCode.OK
            };

            A.CallTo(handler).WithReturnType<Task<HttpResponseMessage>>().WithAnyArguments().Returns(Task.FromResult(httpResponse));

            var client = new PostmatesClient("clientID", "username", handler);

            var response = client.GetDeliveryListAsync().Result;

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseKinds.delivery_list, response.Kind);
            Assert.IsTrue(response.GetResponseObject<DeliveryCollection>() is DeliveryCollection);
            var deliveries = response.GetResponseObject<DeliveryCollection>();
            Assert.IsNotNull(deliveries);
            Assert.IsTrue(deliveries.Deliveries.Any());
        }
        [TestMethod]
        public void LIST_DELIVERIES_ERROR()
        {
            var handler = A.Fake<IHttpHandler>();
            var errorString = "{\"kind\": \"error\",\"code\": \"invalid_params\",\"message\": \"The parameters of your request were invalid.\"}";

            var httpResponse = new HttpResponseMessage {
                Content = new StringContent(errorString),
                StatusCode = System.Net.HttpStatusCode.NotFound
            };

            A.CallTo(handler).WithReturnType<Task<HttpResponseMessage>>().WithAnyArguments().Returns(Task.FromResult(httpResponse));

            var client = new PostmatesClient("clientID", "username", handler);

            var response = client.GetDeliveryListAsync().Result;


            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseKinds.error, response.Kind);
            Assert.IsTrue(response.GetResponseObject<ErrorResponse>() is ErrorResponse);
            var error = response.GetResponseObject<ErrorResponse>();
            Assert.IsTrue(!string.IsNullOrEmpty(error.Code));
            Assert.IsTrue(!string.IsNullOrEmpty(error.Message));
            Assert.AreEqual((int)System.Net.HttpStatusCode.NotFound, error.HttpStatusCodes);
        }
        [TestMethod]
        public void GET_A_DELIVERY_MUST_HAVE_NOT_NULL_OR_EMOTY_DELIVERY_ID() =>
            Assert.ThrowsException<AggregateException>(() => new PostmatesClient("clientID", "username").GetDeliveryAsync(null).Result);
        [TestMethod]
        [DeploymentItem("DELIVERY.json", "targetFolder")]
        public void GET_A_DELIVERY()
        {
            var handler = A.Fake<IHttpHandler>();
            var responseData = System.IO.File.ReadAllText(@"targetFolder\DELIVERY.json");

            var httpResponse = new HttpResponseMessage {
                Content = new StringContent(responseData),
                StatusCode = System.Net.HttpStatusCode.OK
            };

            A.CallTo(handler).WithReturnType<Task<HttpResponseMessage>>().WithAnyArguments().Returns(Task.FromResult(httpResponse));

            var client = new PostmatesClient("clientID", "username", handler);

            var response = client.GetDeliveryAsync("DeliveryId").Result;

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseKinds.delivery, response.Kind);
            Assert.IsTrue(response.GetResponseObject<Delivery>() is Delivery);
            var develery = response.GetResponseObject<Delivery>();
            Assert.IsNotNull(develery);
        }
        [TestMethod]
        public void GET_A_DELIVERY_ERROR()
        {
            var handler = A.Fake<IHttpHandler>();
            var errorString = "{\"kind\": \"error\",\"code\": \"invalid_params\",\"message\": \"The parameters of your request were invalid.\"}";

            var httpResponse = new HttpResponseMessage {
                Content = new StringContent(errorString),
                StatusCode = System.Net.HttpStatusCode.NotFound
            };

            A.CallTo(handler).WithReturnType<Task<HttpResponseMessage>>().WithAnyArguments().Returns(Task.FromResult(httpResponse));

            var client = new PostmatesClient("clientID", "username", handler);

            var response = client.GetDeliveryAsync("DeliveryId").Result;


            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseKinds.error, response.Kind);
            Assert.IsTrue(response.GetResponseObject<ErrorResponse>() is ErrorResponse);
            var error = response.GetResponseObject<ErrorResponse>();
            Assert.IsTrue(!string.IsNullOrEmpty(error.Code));
            Assert.IsTrue(!string.IsNullOrEmpty(error.Message));
            Assert.AreEqual((int)System.Net.HttpStatusCode.NotFound, error.HttpStatusCodes);
        }
        [TestMethod]
        public void CANCEL_A_DELIVERY_MUST_HAVE_NOT_NULL_OR_EMOTY_DELIVERY_ID() =>
           Assert.ThrowsException<AggregateException>(() => new PostmatesClient("clientID", "username").CancelDeliveryAsync(null).Result);
        [TestMethod]
        [DeploymentItem("DELIVERY.json", "targetFolder")]
        public void CANCEL_A_DELIVERY()
        {
            var handler = A.Fake<IHttpHandler>();
            var responseData = System.IO.File.ReadAllText(@"targetFolder\DELIVERY.json");

            var httpResponse = new HttpResponseMessage {
                Content = new StringContent(responseData),
                StatusCode = System.Net.HttpStatusCode.OK
            };

            A.CallTo(handler).WithReturnType<Task<HttpResponseMessage>>().WithAnyArguments().Returns(Task.FromResult(httpResponse));

            var client = new PostmatesClient("clientID", "username", handler);

            var response = client.CancelDeliveryAsync("DeliveryId").Result;

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseKinds.delivery, response.Kind);
            Assert.IsTrue(response.GetResponseObject<Delivery>() is Delivery);
            var develery = response.GetResponseObject<Delivery>();
            Assert.IsNotNull(develery);
        }
        [TestMethod]
        public void CANCEL_A_DELIVERY_ERROR()
        {
            var handler = A.Fake<IHttpHandler>();
            var errorString = "{\"kind\": \"error\",\"code\": \"invalid_params\",\"message\": \"The parameters of your request were invalid.\"}";

            var httpResponse = new HttpResponseMessage {
                Content = new StringContent(errorString),
                StatusCode = System.Net.HttpStatusCode.NotFound
            };

            A.CallTo(handler).WithReturnType<Task<HttpResponseMessage>>().WithAnyArguments().Returns(Task.FromResult(httpResponse));

            var client = new PostmatesClient("clientID", "username", handler);

            var response = client.CancelDeliveryAsync("DeliveryId").Result;


            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseKinds.error, response.Kind);
            Assert.IsTrue(response.GetResponseObject<ErrorResponse>() is ErrorResponse);
            var error = response.GetResponseObject<ErrorResponse>();
            Assert.IsTrue(!string.IsNullOrEmpty(error.Code));
            Assert.IsTrue(!string.IsNullOrEmpty(error.Message));
            Assert.AreEqual((int)System.Net.HttpStatusCode.NotFound, error.HttpStatusCodes);
        }
        [TestMethod]
        public void ADD_A_TIP_TO_A_DELIVERY_MUST_HAVE_NOT_NULL_OR_EMOTY_DELIVERY_ID() =>
          Assert.ThrowsException<AggregateException>(() => new PostmatesClient("clientID", "username").AddTipAsync(null,1).Result);
        
        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        public void ADD_A_TIP_TO_A_DELIVERY_MUST_HAVE_TIP_MORE_THAN_0(int tip) =>
          Assert.ThrowsException<AggregateException>(() => new PostmatesClient("clientID", "username").AddTipAsync("DeliveryId", tip).Result);
        [TestMethod]
        [DeploymentItem("DELIVERY.json", "targetFolder")]
        public void ADD_A_TIP_TO_A_DELIVERY()
        {
            var handler = A.Fake<IHttpHandler>();
            var responseData = System.IO.File.ReadAllText(@"targetFolder\DELIVERY.json");

            var httpResponse = new HttpResponseMessage {
                Content = new StringContent(responseData),
                StatusCode = System.Net.HttpStatusCode.OK
            };

            A.CallTo(handler).WithReturnType<Task<HttpResponseMessage>>().WithAnyArguments().Returns(Task.FromResult(httpResponse));

            var client = new PostmatesClient("clientID", "username", handler);

            var response = client.AddTipAsync("DeliveryId",1).Result;

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseKinds.delivery, response.Kind);
            Assert.IsTrue(response.GetResponseObject<Delivery>() is Delivery);
            var develery = response.GetResponseObject<Delivery>();
            Assert.IsNotNull(develery);
        }
        [TestMethod]
        public void ADD_A_TIP_TO_A_DELIVERY_ERROR()
        {
            var handler = A.Fake<IHttpHandler>();
            var errorString = "{\"kind\": \"error\",\"code\": \"invalid_params\",\"message\": \"The parameters of your request were invalid.\"}";

            var httpResponse = new HttpResponseMessage {
                Content = new StringContent(errorString),
                StatusCode = System.Net.HttpStatusCode.NotFound
            };

            A.CallTo(handler).WithReturnType<Task<HttpResponseMessage>>().WithAnyArguments().Returns(Task.FromResult(httpResponse));

            var client = new PostmatesClient("clientID", "username", handler);

            var response = client.AddTipAsync("DeliveryId",1).Result;


            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseKinds.error, response.Kind);
            Assert.IsTrue(response.GetResponseObject<ErrorResponse>() is ErrorResponse);
            var error = response.GetResponseObject<ErrorResponse>();
            Assert.IsTrue(!string.IsNullOrEmpty(error.Code));
            Assert.IsTrue(!string.IsNullOrEmpty(error.Message));
            Assert.AreEqual((int)System.Net.HttpStatusCode.NotFound, error.HttpStatusCodes);
        }
        [TestMethod]
        [DeploymentItem("WEBHOOK_EVENT.json", "targetFolder")]
        public void WEBHOOK_DESERIALIZE()
        {
            var responseData = System.IO.File.ReadAllText(@"targetFolder\WEBHOOK_EVENT.json");
            var webhookEvent = Newtonsoft.Json.JsonConvert.DeserializeObject<WebHookEvent>(responseData);
            
            Assert.AreEqual(webhookEvent.DeliveryId, "del_3vDjjkd21b");
            Assert.AreEqual(webhookEvent.Id, "evt_KC9LLdlTwr7udF");
            Assert.AreEqual(webhookEvent.Kind,  ResponseKinds.event_delivery_status);
            Assert.IsNotNull(webhookEvent.Data);
            Assert.IsTrue(webhookEvent.Data is Delivery);
        }
        //[TestMethod]
        //public void CancelDeliveryListAsync_intergration()
        //{
        //    var cli = new PostmatesClient("cus_Kf3bMZuhfEUbQV", "effcda92-ecc3-4db6-b954-f8d914e094d9");
        //    var r = cli.CancelDeliveryAsync("del_LOEOHt3oTjl46V").Result;

        //    if (r.Kind == ResponseKinds.delivery) {
        //        var dq = r.GetResponseObject<Delivery>();
        //    }
        //    else {
        //        var err = r.GetResponseObject<ErrorResponse>();
        //    }
        //}

        //[TestMethod]
        //public void GetDeliveryListAsync_intergration()
        //{
        //    var cli = new PostmatesClient("cus_Kf3bMZuhfEUbQV", "effcda92-ecc3-4db6-b954-f8d914e094d9");
        //    var r = cli.GetDeliveryAsync("del_LOEOHt3oTjl46V").Result;

        //    if (r.Kind == ResponseKinds.delivery) {
        //        var dq = r.GetResponseObject<Delivery>();
        //    }
        //    else {
        //        var err = r.GetResponseObject<ErrorResponse>();
        //    }
        //}

        //[TestMethod]
        //public void GetDeliveryListAsync_intergration()
        //{
        //    var cli = new PostmatesClient("cus_Kf3bMZuhfEUbQV", "effcda92-ecc3-4db6-b954-f8d914e094d9");
        //    var r = cli.GetDeliveryListAsync("ongoing", "/v1/customers/cus_Kf3bMZuhfEUbQV/deliveries?filter=&limit=32&offset=32").Result;

        //    if (r.Kind == ResponseKinds.delivery_list) {
        //        var dq = r.GetResponseObject<DeliveryCollection>();
        //    }
        //    else {
        //        var err = r.GetResponseObject<ErrorResponse>();
        //    }
        //}

        //[TestMethod]
        //public void GetDeliveryZonesAsync_intergration()
        //{
        //    var cli = new PostmatesClient("cus_Kf3bMZuhfEUbQV", "effcda92-ecc3-4db6-b954-f8d914e094d9");
        //    var r = cli.GetDeliveryZonesAsync().Result;

        //    if (r.Kind == ResponseKinds.delivery_zones) {
        //        var dq = r.GetResponseObject<DeliveryZoneCollection>();
        //    }
        //    else {
        //        var err = r.GetResponseObject<ErrorResponse>();
        //    }
        //}
        //}
        //[TestMethod]
        //public void GetQote_intergration()
        //{
        //    var cli = new PostmatesClient("cus_Kf3bMZuhfEUbQV", "effcda92-ecc3-4db6-b954-f8d914e094d9");
        //    var r = cli.GetQuoteAsync(new Address {
        //        StreetNumber = "270 Landfall Rd",
        //        City = "Atlanta",
        //        State = "GA"
        //    },
        //     new Address {
        //         StreetNumber = "",
        //         City = "",
        //         State = ""
        //     }
        //     ).Result;

        //    if (r.Kind == ResponseKinds.delivery_quote) {
        //        var dq = r.GetResponseObject<DeliveryQuote>();
        //    }
        //    else {
        //        var err = r.GetResponseObject<ErrorResponse>();
        //    }

        //}
    }
}