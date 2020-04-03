using Postman_API.Models.Environments;
using Newtonsoft.Json;

namespace Postman_API.Services
{
    class EnvironmentService : WebClientRequests
    {
        public EnvironmentService() : base() { }

        public EnvironmentListInfoModel GetAllCollections()
        {
            return Get<EnvironmentListInfoModel>(Constants.baseUri + "/environments");
        }

        public EnvironmentContentModel GetSingleCollection(string uid)
        {
            return Get<EnvironmentContentModel>(Constants.baseUri + "/environments" + $"/{uid}");
        }

        public EnvironmentInfoModel CreateCollection(EnvironmentContentModel content)
        {
            var serializedContend = JsonConvert.SerializeObject(content);
            return Post<EnvironmentInfoModel>(Constants.baseUri + "/environments", serializedContend);
        }

        public EnvironmentInfoModel UpdateCollection(EnvironmentContentModel content, string uid)
        {
            var serializedContend = JsonConvert.SerializeObject(content);
            return Put<EnvironmentInfoModel>(Constants.baseUri + "/environments" + $"/{uid}", serializedContend);
        }

        public EnvironmentInfoModel DeleteCollection(string uid)
        {
            return Delete<EnvironmentInfoModel>(Constants.baseUri + "/environments" + $"/{uid}");
        }
    }
}
