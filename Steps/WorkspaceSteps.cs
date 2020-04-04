using System;
using System.Linq;
using System.Collections.Generic;
using Postman_API.Models.Workspaces;
using Postman_API.Models.Environments;
using Postman_API.Models.Collections;
using Postman_API.Services;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Postman_API.Steps
{
    [Binding, Scope(Feature = "Workspaces")]
    class WorkspaceSteps
    {
        [Given(@"I have workspace with name (.*)")]
        [When(@"I send request to create (.*) workspace")]
        public void IHaveWorkspaceWithName(string workspaceName)
        {
            var contentModel = CreateModel(workspaceName);
            var postResponse = new WorkspaceService().CreateWorkspace(contentModel);
            ScenarioContext.Current["wsPostResponse"] = postResponse;
        }

        [Given(@"I have workspace (.*) which contains this collection")]
        public void IHaveWorkspaceWhichContainsRequest(string workspaceName)
        {
            var clPostResponse = ScenarioContext.Current["clPostResponse"] as CollectionInfoModel;
            var collectionList = new List<CollectionInfo> { clPostResponse.collection };
            var contentModel = CreateModel(workspaceName, collectionList:collectionList);
            var postResponse = new WorkspaceService().CreateWorkspace(contentModel);
            ScenarioContext.Current["collectionList"] = collectionList;
            ScenarioContext.Current["wsPostResponse"] = postResponse;
        }

        [When(@"I send GET request to postman workspaces api")]
        public void WhenISendGETRequestToPostmanWorkspacesApi()
        {
            var getAllResponse = new WorkspaceService().GetAllWorkspaces();
            ScenarioContext.Current["wsGetAllResponse"] = getAllResponse;
        }

        [When(@"I send request to rename this workspace to (.*)")]
        public void WhenISendRequestToRenameWorkspaceTo(string newName)
        {
            var postResponse = ScenarioContext.Current["wsPostResponse"] as WorkspaceInfoModel;
            var contentModel = CreateModel(newName, postResponse.workspace.id);
            new WorkspaceService().UpdateWorkspace(contentModel, postResponse.workspace.id);
        }

        [When(@"I send request to delete this workspace")]
        public void WhenISendRequestToDeleteWorkspace()
        {
            var postResponse = ScenarioContext.Current["wsPostResponse"] as WorkspaceInfoModel;
            new WorkspaceService().DeleteWorkspace(postResponse.workspace.id);
        }

        [When(@"I send GET request to postman workspaces api with id of this workspace")]
        public void WhenISendGETRequestToPostmanWorkspacesApiWithIdOfThisWorkspace()
        {
            var postResponse = ScenarioContext.Current["wsPostResponse"] as WorkspaceInfoModel;
            var getSingleResponse = new WorkspaceService().GetSingleWorkspace(postResponse.workspace.id);
            ScenarioContext.Current["wsGetSingleResponse"] = getSingleResponse;
        }

        [Then(@"I get list of workspaces with (.*) name in it")]
        public void ThenIGetListOfWorkspacesWithNameInIt(string name)
        {
            var getAllResponse = ScenarioContext.Current["wsGetAllResponse"] as WorkspaceListInfoModel;
            getAllResponse.workspaces.Any(i => i.name.Equals(name)).Should().BeTrue();
        }

        [Then(@"Created workspace appears in postman")]
        public void ThenCreatedWorkspaceAppearsInPostman()
        {
            var getAllResponse = new WorkspaceService().GetAllWorkspaces();
            var postResponse = ScenarioContext.Current["wsPostResponse"] as WorkspaceInfoModel;
            getAllResponse.workspaces.Any(i => i.id.Equals(postResponse.workspace.id)).Should().BeTrue();
        }

        [Then(@"Name of workspace was updated to (.*)")]
        public void ThenNameOfWorkspaceWasUpdatedTo(string newName)
        {
            var getAllResponse = new WorkspaceService().GetAllWorkspaces();
            var postResponse = ScenarioContext.Current["wsPostResponse"] as WorkspaceInfoModel;
            getAllResponse.workspaces.FirstOrDefault(i => i.id.Equals(postResponse.workspace.id)).name.Should().Equals(newName);
        }

        [Then(@"Workspace was deleted")]
        public void ThenWorkspaceWasDeleted()
        {
            var getAllResponse = new WorkspaceService().GetAllWorkspaces();
            var postResponse = ScenarioContext.Current["wsPostResponse"] as WorkspaceInfoModel;
            getAllResponse.workspaces.All(i => i.id.Equals(postResponse.workspace.id)).Should().BeFalse();
        }

        [Then(@"I get response with workspace content")]
        public void ThenIGetResponseWithWorkspaceContent()
        {
            var postResponse = ScenarioContext.Current["wsPostResponse"] as WorkspaceInfoModel;
            var collectionList = ScenarioContext.Current["collectionList"] as List<CollectionInfo>;
            var getSingleResponse = ScenarioContext.Current["wsGetSingleResponse"] as WorkspaceContentModel;

            getSingleResponse.workspace.name.Equals(postResponse.workspace.name).Should().BeTrue();
            getSingleResponse.workspace.collections.Select(i => i.uid).SequenceEqual(collectionList.Select(i => i.uid)).Should().BeTrue();
        }

        private WorkspaceContentModel CreateModel(string workspaceName = null, string id = null, string type = null, string description = null,
                                                    List<CollectionInfo> collectionList = null, List<EnvironmentInfo> environmentList = null,
                                                    List<Mock> mockList = null, List<Monitor> monitorList = null)
        {
            Workspace workspace = new Workspace
            {
                id = id ?? Guid.NewGuid().ToString(),
                name = workspaceName ?? "vsTest",
                type = type ?? "personal",
                description = description ?? "Test",
                collections = collectionList ?? new List<CollectionInfo> { },
                environments = environmentList ?? new List<EnvironmentInfo> { },
                mocks = mockList ?? new List<Mock> { },
                monitors = monitorList ?? new List<Monitor> { }
            };

            return new WorkspaceContentModel { workspace = workspace };
        }
    }
}
