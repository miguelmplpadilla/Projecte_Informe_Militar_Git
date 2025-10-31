using System;
using System.Collections.Generic;
using UnityEngine;

namespace Resources.Scripts.Inventory
{
    [CreateAssetMenu(fileName = "Inventory", menuName = "ScriptableObjects/Inventory")]
    public class InventoryData : ScriptableObject
    {
        public List<ItemData> items = new List<ItemData>();
    }

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