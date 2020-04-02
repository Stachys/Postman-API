using System.Linq;
using System.Collections.Generic;
using Postman_API.Services.Collections;
using TechTalk.SpecFlow;

namespace Postman_API.Steps.Collections
{
    [Binding, Scope(Feature = "Collections")]
    static class CollectionHooks
    {
        [AfterScenario]
        public static void DeleteTestCollections()
        {
            var collectionListInfo = new CollectionService().GetCollections();
            IEnumerable<string> uidToDel = collectionListInfo.collections.Where(i => i.name.Contains("vsTest")).Select(i => i.uid);
            foreach (string uid in uidToDel)
            {
                new CollectionService().DeleteCollection(uid);
            }
        }
    }
}
