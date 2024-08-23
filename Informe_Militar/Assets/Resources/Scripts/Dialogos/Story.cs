using System;
using System.Collections.Generic;

namespace Resources.Scripts.Dialogos
{
    [Serializable]
    public class Story
    {
        public string id;
        public string text;
        public List<string> tags;
        public List<string> vars;
        public string speaker = "";
        public bool ussed;
    }

    [Serializable]
    public class RootStory
    {
        public List<Story> storys;
    }
}