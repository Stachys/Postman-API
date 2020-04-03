using System.Collections.Generic;

namespace Postman_API.Models.Environments
{
    class EnvironmentContentModel
    {
        public Environment environment { get; set; }
    }

    public class Environment
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<Value> values { get; set; }
    }

    public class Value
    {
        public string key { get; set; }
        public string value { get; set; }
        public string type { get; set; }
        public bool enabled { get; set; }
        public bool hovered { get; set; }
    }
}
