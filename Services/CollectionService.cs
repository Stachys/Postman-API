using Postman_API.Models.Collections;
using Newtonsoft.Json;

namespace Postman_API.Services
{
    class CollectionService : WebClientRequests
    {
        public CollectionService() : base() { }

        public CollectionListInfoModel GetAllCollections()
        {
            return Get<CollectionListInfoModel>(Constants.baseUri + "/collections");
        }

        public CollectionContentModel GetSingleCollection(string uid)
        {
            return Get<CollectionContentModel>(Constants.baseUri + "/collections" + $"/{uid}");
        }

        public CollectionInfoModel CreateCollection(CollectionContentModel content)
        {
            var serializedContend = JsonConvert.SerializeObject(content);
            return Post<CollectionInfoModel>(Constants.baseUri + "/collections", serializedContend);
        }

        public CollectionInfoModel CreateFork(CreateForkModel content, string collectionUid, string workspaceId)
        {
            var serializedContend = JsonConvert.SerializeObject(content);
            return Post<CollectionInfoModel>(Constants.baseUri + $"/collections/fork/{collectionUid}/?workspace={workspaceId}", serializedContend);
        }

        public CollectionInfoModel MergeFork(MergeForkModel content)
        {
            var serializedContend = JsonConvert.SerializeObject(content);
            return Post<CollectionInfoModel>(Constants.baseUri + "/collections/merge", serializedContend);
        }

        public CollectionInfoModel UpdateCollection(CollectionContentModel content, string uid)
        {
            var serializedContend = JsonConvert.SerializeObject(content);
            return Put<CollectionInfoModel>(Constants.baseUri + "/collections" + $"/{uid}", serializedContend);
        }

        public CollectionInfoModel DeleteCollection(string uid)
        {
            return Delete<CollectionInfoModel>(Constants.baseUri + "/collections" + $"/{uid}");
        }
    }
}
