namespace Postman_API.Models.Collections
{
    class CollectionInfo
    {
        public string id { get; set; }
        public string name { get; set; }
        public string owner { get; set; }
        public string uid { get; set; }
        public Fork fork { get; set; }
    }
    class Fork
    {
        public string label { get; set; }
        public string createdAt { get; set; }
        public string from { get; set; }
    }
}
