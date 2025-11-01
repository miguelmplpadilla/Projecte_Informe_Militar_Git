using System;
using UnityEngine;

namespace Resources.Scripts.Inventory
{
    [Serializable]
    public class ItemData
    {
        public string keyItem;
        public LocalizableString nameItem;
        public LocalizableString descriptionItem;
        public int cantItem;
        public GameObject prefabItem;
        
        public ItemData(ItemData itemData)
        {
            keyItem = itemData.keyItem;
            nameItem = itemData.nameItem;
            descriptionItem = itemData.descriptionItem;
            cantItem = itemData.cantItem;
            prefabItem = itemData.prefabItem;
        }
    }
}