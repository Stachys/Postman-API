using Postman_API.Models.Environments;
using Newtonsoft.Json;

namespace Postman_API.Services
{
    class EnvironmentService : WebClientRequests
    {
        public EnvironmentService() : base() { }

        public EnvironmentListInfoModel GetAllEnvironments()
        {
            return Get<EnvironmentListInfoModel>(Constants.baseUri + "/environments");
        }

        public EnvironmentContentModel GetSingleEnvironment(string uid)
        {
            return Get<EnvironmentContentModel>(Constants.baseUri + "/environments" + $"/{uid}");
        }

        public EnvironmentInfoModel CreateEnvironment(EnvironmentContentModel content)
        {
            var serializedContend = JsonConvert.SerializeObject(content);
            return Post<EnvironmentInfoModel>(Constants.baseUri + "/environments", serializedContend);
        }

        public EnvironmentInfoModel UpdateEnvironment(EnvironmentContentModel content, string uid)
        {
            var serializedContend = JsonConvert.SerializeObject(content);
            return Put<EnvironmentInfoModel>(Constants.baseUri + "/environments" + $"/{uid}", serializedContend);
        }

        public EnvironmentInfoModel DeleteEnvironment(string uid)
        {
            return Delete<EnvironmentInfoModel>(Constants.baseUri + "/environments" + $"/{uid}");
        }
    }
}
