using System;
using System.Linq;
using System.Collections.Generic;
using Postman_API.Models.Environments;
using Postman_API.Services;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Postman_API.Steps
{
    [Binding, Scope(Feature = "Environments")]
    class EnvironmentSteps
    {
        [Scope(Tag = "Environment")]
        [Given(@"I have environment with name (.*)")]
        [When(@"I send request to create (.*) environment")]
        public void IHaveEnvironmentWithName(string environmentName)
        {
            var contentModel = CreateModel(environmentName);
            var postResponse = new EnvironmentService().CreateEnvironment(contentModel);
            ScenarioContext.Current["envPostResponse"] = postResponse;
        }

        [Given(@"I have environment (.*) with variables")]
        public void IHaveEnvironmentWhichContainsRequest(string environmentName)
        {
            var variables = new List<Value>
            {
                new Value { key = "Test", value = "Test", type = "text", enabled = true, hovered = false }
            };
            var contentModel = CreateModel(environmentName, variables:variables);
            var postResponse = new EnvironmentService().CreateEnvironment(contentModel);
            ScenarioContext.Current["variableList"] = variables;
            ScenarioContext.Current["envPostResponse"] = postResponse;
        }

        [When(@"I send GET request to postman environments api")]
        public void WhenISendGETRequestToPostmanEnvironmentsApi()
        {
            var getAllResponse = new EnvironmentService().GetAllEnvironments();
            ScenarioContext.Current["envGetAllResponse"] = getAllResponse;
        }

        [When(@"I send request to rename this environment to (.*)")]
        public void WhenISendRequestToRenameEnvironmentTo(string newName)
        {
            var postResponse = ScenarioContext.Current["envPostResponse"] as EnvironmentInfoModel;
            var contentModel = CreateModel(newName, postResponse.environment.id);
            new EnvironmentService().UpdateEnvironment(contentModel, postResponse.environment.uid);
        }

        [When(@"I send request to delete this environment")]
        public void WhenISendRequestToDeleteEnvironment()
        {
            var postResponse = ScenarioContext.Current["envPostResponse"] as EnvironmentInfoModel;
            new EnvironmentService().DeleteEnvironment(postResponse.environment.uid);
        }

        [When(@"I send GET request to postman environments api with uid of this environment")]
        public void WhenISendGETRequestToPostmanEnvironmentsApiWithUidOfThisEnvironment()
        {
            var postResponse = ScenarioContext.Current["envPostResponse"] as EnvironmentInfoModel;
            var getSingleResponse = new EnvironmentService().GetSingleEnvironment(postResponse.environment.uid);
            ScenarioContext.Current["envGetSingleResponse"] = getSingleResponse;
        }

        [Then(@"I get list of environments with (.*) name in it")]
        public void ThenIGetListOfEnvironmentsWithNameInIt(string name)
        {
            var getAllResponse = ScenarioContext.Current["envGetAllResponse"] as EnvironmentListInfoModel;
            getAllResponse.environments.Any(i => i.name.Equals(name)).Should().BeTrue();
        }

        [Then(@"Created environment appears in postman")]
        public void ThenCreatedEnvironmentAppearsInPostman()
        {
            var getAllResponse = new EnvironmentService().GetAllEnvironments();
            var postResponse = ScenarioContext.Current["envPostResponse"] as EnvironmentInfoModel;
            getAllResponse.environments.Any(i => i.uid.Equals(postResponse.environment.uid)).Should().BeTrue();
        }

        [Then(@"Name of environment was updated to (.*)")]
        public void ThenNameOfEnvironmentWasUpdatedTo(string newName)
        {
            var getAllResponse = new EnvironmentService().GetAllEnvironments();
            var postResponse = ScenarioContext.Current["envPostResponse"] as EnvironmentInfoModel;
            getAllResponse.environments.FirstOrDefault(i => i.uid.Equals(postResponse.environment.uid)).name.Should().Equals(newName);
        }

        [Then(@"Environment was deleted")]
        public void ThenEnvironmentWasDeleted()
        {
            var getAllResponse = new EnvironmentService().GetAllEnvironments();
            var postResponse = ScenarioContext.Current["envPostResponse"] as EnvironmentInfoModel;
            getAllResponse.environments.All(i => i.uid.Equals(postResponse.environment.uid)).Should().BeFalse();
        }

        [Then(@"I get response with environment content")]
        public void ThenIGetResponseWithEnvironmentContent()
        {
            var postResponse = ScenarioContext.Current["envPostResponse"] as EnvironmentInfoModel;
            var variableList = ScenarioContext.Current["variableList"] as List<Value>;
            var getSingleResponse = ScenarioContext.Current["envGetSingleResponse"] as EnvironmentContentModel;

            getSingleResponse.environment.id.Equals(postResponse.environment.id).Should().BeTrue();
            getSingleResponse.environment.name.Equals(postResponse.environment.name).Should().BeTrue();
            getSingleResponse.environment.values.Select(i => new { i.key, i.value}).SequenceEqual(variableList.Select(i => new { i.key, i.value })).Should().BeTrue();
        }

        private EnvironmentContentModel CreateModel(string name = null, string id = null, List<Value> variables = null)
        {
            var environment = new Models.Environments.Environment
            {
                id = id ?? Guid.NewGuid().ToString(),
                name = name ?? "vsTest",
                values = variables ?? new List<Value> { }
            };

            return new EnvironmentContentModel { environment = environment };
        }
    }
}
