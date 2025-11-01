using System;
using System.Collections;
using System.Collections.Generic;
using Resources.Scripts.Inventory;
using Resources.Scripts.Tools;
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

    public LocalizableController textDescription;

    public List<ItemInventoryManager> itemsInventory = new List<ItemInventoryManager>();
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(CreateItemsStart());
    }

    private void Update()
    {
        //backpack.SetActive(current3DObj == null);
    }

    private IEnumerator CreateItemsStart()
    {
        foreach (var itemData in inventoryData.items)
            itemsInventory.Add(CreateItem(itemData));

        yield return null;
        
        SetObj3D(itemsInventory[0]);
    }

    private ItemInventoryManager CreateItem(ItemData itemData)
    {
        GameObject itemObj = Instantiate(itemPrefab, continer.transform);
        ItemInventoryManager itemManager = itemObj.GetComponent<ItemInventoryManager>();
        itemManager.SetData(itemData);

        return itemManager;
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
        
        textDescription.SetText(itemManager.data.descriptionItem);
        
        itemManager.selector.transform.localScale = Vector3.one;
        GameObject obj3D = Instantiate(itemManager.data.prefabItem, continer3DObj.transform);
        current3DObj = obj3D;
    }

    public void AddObjectToInventory(ItemData itemData)
    {
        foreach (var itemInventory in itemsInventory)
        {
            if (itemData.keyItem.Equals(itemInventory.data.keyItem))
            {
                itemInventory.data.cantItem += itemData.cantItem;
                return;
            }
        }
        
        itemsInventory.Add(CreateItem(itemData));
    }

    public void UnlockPoster(DiapositiveNode.DataDiapositive dataPoster)
    {
        
    }
}
