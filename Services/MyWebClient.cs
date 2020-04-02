using System.Net;

namespace Postman_API.Services
{
    class MyWebClient : WebClient
    {
        private HttpWebResponse _extendedResponse;
        protected HttpWebResponse HttpWebResponse
        {
            get { return _extendedResponse; }
            private set { _extendedResponse = value; }
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            WebResponse response = base.GetWebResponse(request);
            _extendedResponse = (HttpWebResponse)response;
            return response;
        }
    }
}