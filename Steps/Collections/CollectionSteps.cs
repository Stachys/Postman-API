using System;
using System.Linq;
using System.Collections.Generic;
using Postman_API.Models.Collections;
using Postman_API.Services;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Postman_API.Steps.Collections
{
    [Binding, Scope(Feature = "Collections")]
    class CollectionSteps
    {
        [Given(@"I have collection with name (.*)")]
        [When(@"I send request to create (.*) collection")]
        public void IHaveCollectionWithName(string collectionName)
        {
            var contentModel = CreateModel(collectionName);
            var postResponse = new CollectionService().CreateCollection(contentModel);
            ScenarioContext.Current["postResponse"] = postResponse;
        }

        [Given(@"I have collection (.*) which contains (.*) request (.*)")]
        public void IHaveCollectionWhichContainsRequest (string collectionName, string method, string requestName)
        {
            var contentModel = CreateModel(collectionName, method, requestName);
            var postResponse = new CollectionService().CreateCollection(contentModel);
            ScenarioContext.Current["postResponse"] = postResponse;
            ScenarioContext.Current["method"] = method;
            ScenarioContext.Current["requestName"] = requestName;
        }

        [When(@"I send GET request to postman collections api")]
        public void WhenISendGETRequestToPostmanCollectionsApi()
        {
            var getAllResponse = new CollectionService().GetAllCollections();
            ScenarioContext.Current["getAllResponse"] = getAllResponse; 
        }

        [When(@"I send request to rename this collection to (.*)")]
        public void WhenISendRequestToRenameCollectionTo(string newName)
        {
            var postResponse = ScenarioContext.Current["postResponse"] as CollectionInfoModel;
            var contentModel = CreateModel(newName);
            new CollectionService().UpdateCollection(contentModel, postResponse.collection.uid);
        }

        [When(@"I send request to delete this collection")]
        public void WhenISendRequestToDeleteCollection()
        {
            var postResponse = ScenarioContext.Current["postResponse"] as CollectionInfoModel;
            new CollectionService().DeleteCollection(postResponse.collection.uid);
        }

        [When(@"I send GET request to postman collections api with uid of this collection")]
        public void WhenISendGETRequestToPostmanCollectionsApiWithUidOfThisCollection()
        {
            var postResponse = ScenarioContext.Current["postResponse"] as CollectionInfoModel;
            var getSingleResponse = new CollectionService().GetSingleCollection(postResponse.collection.uid);
            ScenarioContext.Current["getSingleResponse"] = getSingleResponse;
        }

        [Then(@"I get list of collections with (.*) name in it")]
        public void ThenIGetListOfCollectionsWithNameInIt(string name)
        {
            var getAllResponse = ScenarioContext.Current["getAllResponse"] as CollectionListInfoModel;
            getAllResponse.collections.Any(i => i.name.Equals(name)).Should().BeTrue();
        }

        [Then(@"Created collection appears in postman")]
        public void ThenCreatedCollectionAppearsInPostman()
        {
            var getAllResponse = new CollectionService().GetAllCollections();
            var postResponse = ScenarioContext.Current["postResponse"] as CollectionInfoModel;
            getAllResponse.collections.Any(i => i.uid.Equals(postResponse.collection.uid)).Should().BeTrue();
        }

        [Then(@"Name of collection was updated to (.*)")]
        public void ThenNameOfCollectionWasUpdatedTo(string newName)
        {
            var getAllResponse = new CollectionService().GetAllCollections();
            var postResponse = ScenarioContext.Current["postResponse"] as CollectionInfoModel;
            getAllResponse.collections.FirstOrDefault(i => i.uid.Equals(postResponse.collection.uid)).name.Should().Equals(newName);
        }

        [Then(@"Collection was deleted")]
        public void ThenCollectionWasDeleted()
        {
            var getAllResponse = new CollectionService().GetAllCollections();
            var postResponse = ScenarioContext.Current["postResponse"] as CollectionInfoModel;
            getAllResponse.collections.All(i => i.uid.Equals(postResponse.collection.uid)).Should().BeFalse();
        }

        [Then(@"I get response with collection content")]
        public void ThenIGetResponseWithCollectionContent()
        {
            var postResponse = ScenarioContext.Current["postResponse"] as CollectionInfoModel;
            var getSingleResponse = ScenarioContext.Current["getSingleResponse"] as CollectionContentModel;
            var requestName = ScenarioContext.Current["requestName"];
            var method = ScenarioContext.Current["method"];

            getSingleResponse.collection.info._postman_id.Equals(postResponse.collection.id).Should().BeTrue();
            getSingleResponse.collection.info.name.Equals(postResponse.collection.name).Should().BeTrue();
            getSingleResponse.collection.item.First().name.Equals(requestName).Should().BeTrue();
            getSingleResponse.collection.item.First().request.method.Equals(method).Should().BeTrue();
        }

        private CollectionContentModel CreateModel(string collectionName, string method = "GET", string requestName = "Test")
        {
            var header = new List<Header>
            {
                new Header { key = "X-Api-Key", value = Key.key },
                new Header { key = "Content-Type", value = "application/json" }
            };

            var url = new Url
            {
                raw = Constants.baseUri,
                protocol = "https",
                host = new List<string>
                {
                    "api",
                    "getpostman",
                    "com"
                },
                path = new List<string>
                {
                    "collections"
                }
            };

            var request = new Request
            {
                url = url,
                method = method,
                header = header
            };

            var item = new List<Item>
            {
                new Item
                {
                    name = requestName,
                    request = request
                }
            };

            var info = new Info
            {
                _postman_id = Guid.NewGuid().ToString(),
                name = collectionName,
                schema = Constants.CollectionSchema
            };

            var collection = new CollectionContent
            {
                info = info,
                item = item
            };

            return new CollectionContentModel { collection = collection };
        }
    }
}
