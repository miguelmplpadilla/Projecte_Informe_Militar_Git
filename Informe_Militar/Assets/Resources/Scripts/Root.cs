using System.Collections.Generic;
using Newtonsoft.Json;

namespace Resources.Scripts
{
    public class Link
    {
        public string name { get; set; }
        public string link { get; set; }
        public string pid { get; set; }
    }

    public class Passage
    {
        public string text { get; set; }
        public List<Link> links { get; set; }
        public string name { get; set; }
        public string pid { get; set; }
        public Position position { get; set; }
    }

    public class Position
    {
        public string x { get; set; }
        public string y { get; set; }
    }

    public class Root
    {
        public List<Passage> passages { get; set; }
        public string name { get; set; }
        public string startnode { get; set; }
        public string creator { get; set; }

        [JsonProperty("creator-version")]
        public string creatorversion { get; set; }
        public string ifid { get; set; }
    }
}