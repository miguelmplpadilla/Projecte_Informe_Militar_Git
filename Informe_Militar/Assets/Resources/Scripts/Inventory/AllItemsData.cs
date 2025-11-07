using System.Collections.Generic;
using Resources.Scripts.Inventory;
using UnityEngine;

[CreateAssetMenu(fileName = "AllItemsData", menuName = "ScriptableObjects/AllItemsData")]
public class AllItemsData : ScriptableObject
{
    public List<ItemData> allItems = new List<ItemData>();
}
