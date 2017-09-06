using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PostmatesSDK.Handlers
{
    internal class HttpHandler : IHttpHandler
    {
        private readonly HttpClient _client;
        internal HttpHandler(Uri BaseAddress, string username)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(username + ":")));
            _client.BaseAddress = BaseAddress;
        }
        public Task<HttpResponseMessage> DeleteAsync(string url) => _client.DeleteAsync(url);       
        public Task<HttpResponseMessage> GetAsync(string url) => _client.GetAsync(url);
        public Task<HttpResponseMessage> PostAsync(string url, HttpContent content) => _client.PostAsync(url, content);
        public Task<HttpResponseMessage> PutAsync(string url, HttpContent content) => _client.PutAsync(url, content);
        public void Dispose() => _client.Dispose();
    }
}