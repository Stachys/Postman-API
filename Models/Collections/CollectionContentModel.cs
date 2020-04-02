using System.Collections.Generic;

namespace Postman_API.Models.Collections
{
    class CollectionContentModel
    {
        public CollectionContent collection { get; set; }
    }

    class CollectionContent
    {
        public Info info { get; set; }
        public List<Item> item { get; set; }
    }

    class Info
    {
        public string name { get; set; }
        public string schema { get; set; }
    }

    class Item
    {
        public string name { get; set; }
        public List<Item> item { get; set; }
        public Request request { get; set; }
    }

    class Request
    {
        public string url { get; set; }
        public string method { get; set; }
        public List<Header> header { get; set; }
        public Body body { get; set; }
    }

    class Header
    {
        public string key { get; set; }
        public string value { get; set; }
    }

    class Body
    {
        public string mode { get; set; }
        public string raw { get; set; }
    }
}
