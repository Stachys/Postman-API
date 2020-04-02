using System.Linq;
using System.Collections.Generic;
using Postman_API.Models.Collections;
using Postman_API.Services.Collections;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Postman_API.Steps.Collections
{
    [Binding, Scope(Feature = "Collections")]
    class CollectionSteps
    {
        [Given(@"I have collection with name (.*)")]
        [When(@"I send request to create (.*) collection")]
        public void IHaveCollectionWithName(string name)
        {
            var contentModel = CreateModel(name);
            var postResponse = new CollectionService().CreateCollection(contentModel);
            ScenarioContext.Current["postResponse"] = postResponse;
        }

        [When(@"I send GET request to postman collections api")]
        public void WhenISendGETRequestToPostmanCollectionsApi()
        {
            var getResponse = new CollectionService().GetCollections();
            ScenarioContext.Current["getResponse"] = getResponse; 
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
        
        [Then(@"I get list of collections with (.*) name in it")]
        public void ThenIGetListOfCollectionsWithNameInIt(string name)
        {
            var getResponse = ScenarioContext.Current["getResponse"] as CollectionListInfoModel;
            getResponse.collections.Any(i => i.name.Equals(name)).Should().BeTrue();
        }

        [Then(@"Created collection appears in postman")]
        public void ThenCreatedCollectionAppearsInPostman()
        {
            var getResponse = new CollectionService().GetCollections();
            var postResponse = ScenarioContext.Current["postResponse"] as CollectionInfoModel;
            getResponse.collections.Any(i => i.uid.Equals(postResponse.collection.uid)).Should().BeTrue();
        }

        [Then(@"Name of collection was updated to (.*)")]
        public void ThenNameOfCollectionWasUpdatedTo(string newName)
        {
            var getResponse = new CollectionService().GetCollections();
            var postResponse = ScenarioContext.Current["postResponse"] as CollectionInfoModel;
            getResponse.collections.FirstOrDefault(i => i.uid.Equals(postResponse.collection.uid)).name.Should().Equals(newName);
        }

        [Then(@"Collection was deleted")]
        public void ThenCollectionWasDeleted()
        {
            var getResponse = new CollectionService().GetCollections();
            var postResponse = ScenarioContext.Current["postResponse"] as CollectionInfoModel;
            getResponse.collections.All(i => i.uid.Equals(postResponse.collection.uid)).Should().BeFalse();
        }

        private CollectionContentModel CreateModel(string CollectionName)
        {
            var header = new List<Header>
            {
                new Header { key = "X-Api-Key", value = Key.key },
                new Header { key = "Content-Type", value = "application/json" }
            };

            var request = new Request
            {
                url = Constants.collectionUri,
                method = "GET",
                header = header
            };

            var item = new List<Item>
            {
                new Item
                {
                    name = CollectionName + "request",
                    request = request
                }
            };

            var info = new Info
            {
                name = CollectionName,
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
