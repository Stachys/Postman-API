using System.Collections.Generic;
using Postman_API.Models.Collections;
using Postman_API.Models.Environments;

namespace Postman_API.Models.Workspaces
{
    class WorkspaceContentModel
    {
        public Workspace workspace { get; set; }
    }

    class Workspace
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public List<CollectionInfo> collections { get; set; }
        public List<EnvironmentInfo> environments { get; set; }
        public List<Mock> mocks { get; set; }
        public List<Monitor> monitors { get; set; }
    }
    class Mock
    {
        public string id { get; set; }
    }

    class Monitor
    {
        public string id { get; set; }
    }
}
