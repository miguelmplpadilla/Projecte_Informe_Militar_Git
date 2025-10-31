using System;
using Resources.Scripts.Inventory;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    
    public InventoryData inventoryData;

    public GameObject itemPrefab;

    public GameObject backpack;

    public GameObject continer;
    public GameObject continer3DObj;
    
    private GameObject current3DObj;
    private GameObject lastItemAbove;

    public TextMeshProUGUI textDescription;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CreateItemsStart();
    }

    private void Update()
    {
        //backpack.SetActive(current3DObj == null);
    }

    private void CreateItemsStart()
    {
        foreach (var itemData in inventoryData.items)
            CreateItem(itemData);
    }

    private void CreateItem(ItemData itemData)
    {
        GameObject itemObj = Instantiate(itemPrefab, continer.transform);
        ItemInventoryManager itemManager = itemObj.GetComponent<ItemInventoryManager>();
        itemManager.SetData(itemData);
    }

    public void SetObj3D(ItemInventoryManager itemManager)
    {
        if (current3DObj != null && itemManager != null)
        {
            EventBus<HideSelectorEvent>.Raise(new HideSelectorEvent
            {
                objNotHide = null
            });
            Destroy(current3DObj);
        }

        if (itemManager == null)
        {
            if (lastItemAbove != null)
                EventBus<HideSelectorEvent>.Raise(new HideSelectorEvent
                {
                    objNotHide = lastItemAbove
                });
            return;
        }
        
        lastItemAbove = itemManager.gameObject;

        textDescription.text = itemManager.data.descriptionItem.Value;
        
        itemManager.selector.transform.localScale = Vector3.one;
        GameObject obj3D = Instantiate(itemManager.data.prefabItem, continer3DObj.transform);
        current3DObj = obj3D;
    }
}
