using Newtonsoft.Json;

namespace Postman_API.Services
{
    class WebClientRequests
    {
        protected MyWebClient _webClient;

        protected WebClientRequests()
        {
            _webClient = new MyWebClient();
            _webClient.Headers.Add("X-Api-Key", Key.key);
            _webClient.Headers.Add("Content-Type", "application/json");
        }

        protected T Deserialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }

        protected T Get<T>(string uri) where T : class
        {
            var response = _webClient.DownloadString(uri);
            T deserializedResponse = Deserialize<T>(response);
            return deserializedResponse;
        }

        protected T Post<T>(string uri, string data) where T : class
        {
            var response = _webClient.UploadString(uri, "POST", data);
            T deserializedResponse = Deserialize<T>(response);
            return deserializedResponse;
        }

        protected T Put<T>(string uri, string data) where T : class
        {
            var response = _webClient.UploadString(uri, "PUT", data);
            T deserializedResponse = Deserialize<T>(response);
            return deserializedResponse;
        }

        protected T Delete<T>(string uri, string data = "") where T : class
        {
            var response = _webClient.UploadString(uri, "DELETE", data);
            T deserializedResponse = Deserialize<T>(response);
            return deserializedResponse;
        }

    }
}
