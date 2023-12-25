using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryObjects", menuName = "ScriptableObjects/InventoryObj")]
public class Inventory : ScriptableObject
{
    public List<InvetoryObj> InventoryObjects;
}

[Serializable]
public class InvetoryObj
{
    public string key = "";
    public Sprite spriteInventory = null;

    public bool unlock = false;
}
