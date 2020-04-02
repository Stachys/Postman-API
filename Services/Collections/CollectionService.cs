using Postman_API.Models.Collections;
using Newtonsoft.Json;

namespace Postman_API.Services.Collections
{
    class CollectionService : WebClientRequests
    {
        private const string _uri = "https://api.getpostman.com/collections";

        public CollectionService() : base() { }

        public CollectionListInfoModel GetCollections()
        {
            return Get<CollectionListInfoModel>(_uri);
        }

        public CollectionInfoModel CreateCollection(CollectionContentModel content)
        {
            var serializedContend = JsonConvert.SerializeObject(content);
            return Post<CollectionInfoModel>(_uri, serializedContend);
        }

        public CollectionInfoModel UpdateCollection(CollectionContentModel content, string uid)
        {
            var serializedContend = JsonConvert.SerializeObject(content);
            return Put<CollectionInfoModel>(_uri + $"/{uid}", serializedContend);
        }

        public CollectionInfoModel DeleteCollection(string uid)
        {
            return Delete<CollectionInfoModel>(_uri + $"/{uid}");
        }
    }
}
