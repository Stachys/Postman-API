using Postman_API.Models.Workspaces;
using Newtonsoft.Json;

namespace Postman_API.Services
{
    class WorkspaceService : WebClientRequests
    {
        public WorkspaceService() : base() { }

        public WorkspaceListInfoModel GetAllCollections()
        {
            return Get<WorkspaceListInfoModel>(Constants.baseUri + "/workspaces");
        }

        public WorkspaceContentModel GetSingleCollection(string id)
        {
            return Get<WorkspaceContentModel>(Constants.baseUri + "/workspaces" + $"/{id}");
        }

        public WorkspaceInfoModel CreateCollection(WorkspaceContentModel content)
        {
            var serializedContend = JsonConvert.SerializeObject(content);
            return Post<WorkspaceInfoModel>(Constants.baseUri + "/workspaces", serializedContend);
        }

        public WorkspaceInfoModel UpdateCollection(WorkspaceContentModel content, string id)
        {
            var serializedContend = JsonConvert.SerializeObject(content);
            return Put<WorkspaceInfoModel>(Constants.baseUri + "/workspaces" + $"/{id}", serializedContend);
        }

        public WorkspaceInfoModel DeleteCollection(string id)
        {
            return Delete<WorkspaceInfoModel>(Constants.baseUri + "/workspaces" + $"/{id}");
        }
    }
}
