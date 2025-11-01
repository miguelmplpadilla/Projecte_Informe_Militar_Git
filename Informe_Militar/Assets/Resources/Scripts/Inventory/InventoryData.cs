using System.Collections.Generic;
using Resources.Scripts.Objects;
using UnityEngine;

namespace Resources.Scripts.Inventory
{
    [CreateAssetMenu(fileName = "Inventory", menuName = "ScriptableObjects/Inventory")]
    public class InventoryData : ScriptableObject
    {
        public List<ItemData> items = new List<ItemData>();
        public List<DocumentData> poster = new List<DocumentData>();
        public List<DocumentData> newspapers = new List<DocumentData>();
    }
}