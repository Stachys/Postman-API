using System;
using System.Linq;
using System.Collections.Generic;
using Postman_API.Models.Collections;
using Postman_API.Models.Workspaces;
using Postman_API.Services;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Postman_API.Steps
{
    [Binding, Scope(Feature = "Collections")]
    class CollectionSteps
    {
        [Scope(Tag = "Collection")]
        [Given(@"I have collection with name (.*)")]
        [When(@"I send request to create (.*) collection")]
        public void IHaveCollectionWithName(string collectionName)
        {
            var contentModel = CreateCollectionModel(collectionName);
            var postResponse = new CollectionService().CreateCollection(contentModel);
            ScenarioContext.Current["clPostResponse"] = postResponse;
        }

        [Given(@"I have collection (.*) which contains (.*) request (.*)")]
        public void IHaveCollectionWhichContainsRequest (string collectionName, string method, string requestName)
        {
            var contentModel = CreateCollectionModel(collectionName, method:method, requestName:requestName);
            var postResponse = new CollectionService().CreateCollection(contentModel);
            ScenarioContext.Current["clPostResponse"] = postResponse;
            ScenarioContext.Current["method"] = method;
            ScenarioContext.Current["requestName"] = requestName;
        }

        [When(@"I send GET request to postman collections api")]
        public void WhenISendGETRequestToPostmanCollectionsApi()
        {
            var getAllResponse = new CollectionService().GetAllCollections();
            ScenarioContext.Current["clGetAllResponse"] = getAllResponse; 
        }

        [When(@"I send request to rename this collection to (.*)")]
        public void WhenISendRequestToRenameCollectionTo(string newName)
        {
            var postResponse = ScenarioContext.Current["clPostResponse"] as CollectionInfoModel;
            var contentModel = CreateCollectionModel(newName, postResponse.collection.id);
            new CollectionService().UpdateCollection(contentModel, postResponse.collection.uid);
        }

        [When(@"I send request to delete this collection")]
        public void WhenISendRequestToDeleteCollection()
        {
            var postResponse = ScenarioContext.Current["clPostResponse"] as CollectionInfoModel;
            new CollectionService().DeleteCollection(postResponse.collection.uid);
        }

        [When(@"I send GET request to postman collections api with uid of this collection")]
        public void WhenISendGETRequestToPostmanCollectionsApiWithUidOfThisCollection()
        {
            var postResponse = ScenarioContext.Current["clPostResponse"] as CollectionInfoModel;
            var getSingleResponse = new CollectionService().GetSingleCollection(postResponse.collection.uid);
            ScenarioContext.Current["clGetSingleResponse"] = getSingleResponse;
        }

        [Given(@"I have a fork (.*) of given collection in given workspase")]
        [When(@"I create a fork (.*) of given collection in given workspase")]
        public void WhenICreateAForkOfGivenCollectionToGivenWorkspase(string forkName)
        {
            var clPostResponse = ScenarioContext.Current["clPostResponse"] as CollectionInfoModel;
            var wsPostResponse = ScenarioContext.Current["wsPostResponse"] as WorkspaceInfoModel;
            var content = CreateForkModel(forkName);
            var postResponse = new CollectionService().CreateFork(content, clPostResponse.collection.uid, wsPostResponse.workspace.id);
            ScenarioContext.Current["clPostResponse"] = postResponse;
        }

        [When(@"I merge this fork into collection")]
        public void WhenIMergeAForkIntoCollection()
        {
            var postResponse = ScenarioContext.Current["clPostResponse"] as CollectionInfoModel;
            var content = MergeForkModel(postResponse.collection.uid, postResponse.collection.fork.from);
            new CollectionService().MergeFork(content);
        }

        [Then(@"I get list of collections with (.*) name in it")]
        public void ThenIGetListOfCollectionsWithNameInIt(string name)
        {
            var getAllResponse = ScenarioContext.Current["clGetAllResponse"] as CollectionListInfoModel;
            getAllResponse.collections.Any(i => i.name.Equals(name)).Should().BeTrue();
        }

        [Then(@"Created collection appears in postman")]
        public void ThenCreatedCollectionAppearsInPostman()
        {
            var getAllResponse = new CollectionService().GetAllCollections();
            var postResponse = ScenarioContext.Current["clPostResponse"] as CollectionInfoModel;
            getAllResponse.collections.Any(i => i.uid.Equals(postResponse.collection.uid)).Should().BeTrue();
        }

        [Then(@"Name of collection was updated to (.*)")]
        public void ThenNameOfCollectionWasUpdatedTo(string newName)
        {
            var getAllResponse = new CollectionService().GetAllCollections();
            var postResponse = ScenarioContext.Current["clPostResponse"] as CollectionInfoModel;
            getAllResponse.collections.FirstOrDefault(i => i.uid.Equals(postResponse.collection.uid)).name.Should().Equals(newName);
        }

        [Then(@"Collection was deleted")]
        public void ThenCollectionWasDeleted()
        {
            var getAllResponse = new CollectionService().GetAllCollections();
            var postResponse = ScenarioContext.Current["clPostResponse"] as CollectionInfoModel;
            getAllResponse.collections.All(i => i.uid.Equals(postResponse.collection.uid)).Should().BeFalse();
        }

        [Then(@"I get response with collection content")]
        public void ThenIGetResponseWithCollectionContent()
        {
            var postResponse = ScenarioContext.Current["clPostResponse"] as CollectionInfoModel;
            var getSingleResponse = ScenarioContext.Current["clGetSingleResponse"] as CollectionContentModel;
            var requestName = ScenarioContext.Current["requestName"];
            var method = ScenarioContext.Current["method"];

            getSingleResponse.collection.info._postman_id.Equals(postResponse.collection.id).Should().BeTrue();
            getSingleResponse.collection.info.name.Equals(postResponse.collection.name).Should().BeTrue();
            getSingleResponse.collection.item.First().name.Equals(requestName).Should().BeTrue();
            getSingleResponse.collection.item.First().request.method.Equals(method).Should().BeTrue();
        }

        [Then(@"Created fork appears in postman")]
        public void ThenCreatedForkAppearsInPostman()
        {
            var getAllResponse = new CollectionService().GetAllCollections();
            var postResponse = ScenarioContext.Current["clPostResponse"] as CollectionInfoModel;

            var forkInGetAll = getAllResponse.collections.FirstOrDefault(i => i.uid.Equals(postResponse.collection.uid));
            forkInGetAll.fork.label.Equals(postResponse.collection.fork.label).Should().BeTrue();
            forkInGetAll.fork.from.Equals(postResponse.collection.fork.from).Should().BeTrue();
        }

        [Then(@"Fork was merged")]
        public void ThenForkWasMerged()
        {
            var getAllResponse = new CollectionService().GetAllCollections();
            var postResponse = ScenarioContext.Current["clPostResponse"] as CollectionInfoModel;

            getAllResponse.collections.Any(i => i.uid.Equals(postResponse.collection.uid)).Should().BeFalse();
            getAllResponse.collections.Any(i => i.uid.Equals(postResponse.collection.fork.from)).Should().BeTrue();
        }


        private CreateForkModel CreateForkModel(string label)
        {
            return new CreateForkModel { label = label };
        }

        private MergeForkModel MergeForkModel(string source, string destination, string strategy = null)
        {
            return new MergeForkModel
            {
                strategy = strategy ?? "deleteSource",
                source = source,
                destination = destination
            };
        }

        private CollectionContentModel CreateCollectionModel(string collectionName = null, string id = null, string method = null, string requestName = null)
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
                method = method ?? "GET",
                header = header
            };

            var item = new List<Item>
            {
                new Item
                {
                    name = requestName ?? "Test",
                    request = request
                }
            };

            var info = new Info
            {
                _postman_id = id ?? Guid.NewGuid().ToString(),
                name = collectionName ?? "vsTest",
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
