using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DatacomConsole
{
    public class RestUtility : IRestUtility
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<RestUtility> _logger;

        public RestUtility(IHttpClientFactory httpClientFactory, ILogger<RestUtility> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        /// <summary>
        /// Post request generic method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestContent"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<T> PostRequestAsync<T>(object requestContent, string url)
        {
            T responseObject = default(T);
            try
            {
                var client = _httpClientFactory.CreateClient("MSPServices");
                var response = await client.PostAsync(url, (HttpContent)requestContent);
                var responseJson = await response.Content.ReadAsStringAsync();
                responseObject = JsonConvert.DeserializeObject<T>(responseJson);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured in PostRequestAsync. Message: {ex.Message} Stacktrace: {ex.StackTrace}");
            }
            return responseObject;
        }

        public async Task<HttpStatusCode> GetAsync<T>(string url)
        {
            HttpStatusCode responseStatusCode = HttpStatusCode.NotFound;
            try
            {
                var client = _httpClientFactory.CreateClient("MSPServices");
                var response = await client.GetAsync(url);
                responseStatusCode = response.StatusCode;
            }
            catch (Exception ex)
            {
                //will throw an exception if there is no internet
            }
            return responseStatusCode;
        }

    }
}
