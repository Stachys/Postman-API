using System.Linq;
using System.Collections.Generic;
using Postman_API.Services;
using TechTalk.SpecFlow;

namespace Postman_API.Steps
{
    [Binding]
    static class Hooks
    {
        [AfterScenario]
        [Scope(Feature = "Collections")]
        [Scope(Tag = "Collection")]
        public static void DeleteTestCollections()
        {
            var getAllresponse = new CollectionService().GetAllCollections();
            IEnumerable<string> uidToDel = getAllresponse.collections.Where(i => i.name.Contains("vsTest")).Select(i => i.uid);
            foreach (string uid in uidToDel)
            {
                new CollectionService().DeleteCollection(uid);
            }
        }

        [AfterScenario]
        [Scope(Feature = "Workspaces")]
        [Scope(Tag = "Workspace")]
        public static void DeleteTestWorkspaces()
        {
            var getAllresponse = new WorkspaceService().GetAllWorkspaces();
            IEnumerable<string> idToDel = getAllresponse.workspaces.Where(i => i.name.Contains("vsTest")).Select(i => i.id);
            foreach (string id in idToDel)
            {
                new WorkspaceService().DeleteWorkspace(id);
            }
        }

        [AfterScenario]
        [Scope(Feature = "Environments")]
        [Scope(Tag = "Environment")]
        public static void DeleteTestEnvironments()
        {
            var getAllresponse = new EnvironmentService().GetAllEnvironments();
            IEnumerable<string> uidToDel = getAllresponse.environments.Where(i => i.name.Contains("vsTest")).Select(i => i.uid);
            foreach (string uid in uidToDel)
            {
                new EnvironmentService().DeleteEnvironment(uid);
            }
        }
    }
}
