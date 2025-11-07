using System;
using System.Collections.Generic;

namespace Resources.Scripts.Inventory
{
    [Serializable]
    public class InventoryData
    {
        public List<DataItem> items = new List<DataItem>();
        public List<string> poster = new List<string>();
        public List<string> newspapers = new List<string>();
    }

    [Serializable]
    public class DataItem
    {
        public int cant = 0;
        public string key;
    }
}