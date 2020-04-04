using Postman_API.Models.Workspaces;
using Newtonsoft.Json;

namespace Postman_API.Services
{
    class WorkspaceService : WebClientRequests
    {
        public WorkspaceService() : base() { }

        public WorkspaceListInfoModel GetAllWorkspaces()
        {
            return Get<WorkspaceListInfoModel>(Constants.baseUri + "/workspaces");
        }

        public WorkspaceContentModel GetSingleWorkspace(string id)
        {
            return Get<WorkspaceContentModel>(Constants.baseUri + "/workspaces" + $"/{id}");
        }

        public WorkspaceInfoModel CreateWorkspace(WorkspaceContentModel content)
        {
            var serializedContend = JsonConvert.SerializeObject(content);
            return Post<WorkspaceInfoModel>(Constants.baseUri + "/workspaces", serializedContend);
        }

        public WorkspaceInfoModel UpdateWorkspace(WorkspaceContentModel content, string id)
        {
            var serializedContend = JsonConvert.SerializeObject(content);
            return Put<WorkspaceInfoModel>(Constants.baseUri + "/workspaces" + $"/{id}", serializedContend);
        }

        public WorkspaceInfoModel DeleteWorkspace(string id)
        {
            return Delete<WorkspaceInfoModel>(Constants.baseUri + "/workspaces" + $"/{id}");
        }
    }
}
