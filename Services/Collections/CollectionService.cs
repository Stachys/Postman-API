using Postman_API.Models.Collections;
using Newtonsoft.Json;

namespace Postman_API.Services.Collections
{
    class CollectionService : WebClientRequests
    {
        public CollectionService() : base() { }

        public CollectionListInfoModel GetAllCollections()
        {
            return Get<CollectionListInfoModel>(Constants.collectionUri);
        }

        public CollectionContentModel GetSingleCollection(string uid)
        {
            return Get<CollectionContentModel>(Constants.collectionUri + $"/{uid}");
        }

        public CollectionInfoModel CreateCollection(CollectionContentModel content)
        {
            var serializedContend = JsonConvert.SerializeObject(content);
            return Post<CollectionInfoModel>(Constants.collectionUri, serializedContend);
        }

        public CollectionInfoModel UpdateCollection(CollectionContentModel content, string uid)
        {
            var serializedContend = JsonConvert.SerializeObject(content);
            return Put<CollectionInfoModel>(Constants.collectionUri + $"/{uid}", serializedContend);
        }

        public CollectionInfoModel DeleteCollection(string uid)
        {
            return Delete<CollectionInfoModel>(Constants.collectionUri + $"/{uid}");
        }
    }
}
