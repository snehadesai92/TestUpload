using System;
using System.Configuration;
using System.Net;
using System.Net.Http;

namespace QUERION.Common.Helper
{
    public class WebRequest
    {
        private HttpClient _client;
        private string _webApi;
        public WebClient Client;//=new WebClient();
        public string WebApi;
       
        /// <returns>
        /// httpClient to get the sevices path and consume the services
        /// </returns>
        public HttpClient GetHttpClientData()
        {
            _client = new HttpClient();
            _webApi = ConfigurationManager.AppSettings["WebAPI"];
            _client.BaseAddress = new Uri(_webApi);
            return _client;
        }

       
        /// <returns>
        /// WebClient to get the sevices path and consume the services
        /// </returns>
        public WebClient GetWebClientData()
        {
            //Uri url = HttpContext.Request.UrlReferrer;
            Client = new WebClient();
            WebApi = ConfigurationManager.AppSettings["WebAPI"];
            Client.BaseAddress = new Uri(WebApi).ToString();
            return Client;
        }
    }
}
