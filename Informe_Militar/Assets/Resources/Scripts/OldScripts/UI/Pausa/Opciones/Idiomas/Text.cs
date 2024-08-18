using System;
using System.Collections.Generic;

namespace Resources.Scripts.UI.Idiomas
{
    [Serializable]
    public class Root
    {
        public List<Text> texts = new List<Text>();
    }
    
    [Serializable]
    public class Text
    {
        public string id;
        public string text;
    }
}