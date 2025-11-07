using Resources.Scripts.Inventory;
using Resources.Scripts.Inventory.ItemInventory;
using TMPro;
using UnityEngine;

public class ItemInventoryManager : ItemInventory
{
    public ItemData data;

    public TextMeshProUGUI cantText;
    
    public override void SetData(string itemKey)
    {
        data = InventoryManager.instance.GetItemData(itemKey);
        base.SetData(itemKey);
    }

    protected override void SetText()
    {
        text.SetText(data.nameItem);
    }

    protected override void Update()
    {
        cantText.text = "x" + data.cantItem;
    }
}

public class HideSelectorEvent : IEvent
{
    public GameObject objNotHide;
}