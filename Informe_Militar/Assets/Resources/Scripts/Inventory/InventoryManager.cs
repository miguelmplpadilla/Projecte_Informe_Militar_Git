using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Resources.Scripts.Inventory;
using Resources.Scripts.Inventory.ItemInventory;
using Resources.Scripts.Objects;
using Resources.Scripts.Tools;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    
    public InventoryData inventoryData;

    public CanvasGroup canvasInventory;
    public bool isInventoryShowed = false;

    public GameObject itemPrefab;
    public GameObject itemPosterPrefab;

    public ScrollRect scrollRect;

    public GameObject backpack;

    public GameObject objImage;
    public Image imageDocument;

    public GameObject continerObjects;
    public GameObject continerPosters;
    
    public GameObject continer3DObj;
    
    private GameObject current3DObj;
    private GameObject lastItemAbove;

    public LocalizableController textDescription;

    public List<ItemInventoryManager> itemsInventory = new List<ItemInventoryManager>();
    public List<PosterItemInventoryManager> postersInventory = new List<PosterItemInventoryManager>();
    
    public AllItemsData allItemsData;
    public DocumentsData documentsData;
    
    public Category currentCategory = Category.OBJECTS;
    
    public List<Button> categoryButtons = new List<Button>();

    public enum Category
    {
        OBJECTS, POSTERS, DOCUMENTS, NEWSPAPERS
    }
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        PlayerPrefs.DeleteKey("InventoryData");
        string jsonInventoryData = PlayerPrefs.GetString("InventoryData");
        JsonUtility.FromJsonOverwrite(jsonInventoryData, inventoryData);
        
        StartCoroutine(CreateItemsStart());

        for (int i = 0; i < categoryButtons.Count; i++)
        {
            int index = i;
            categoryButtons[i].onClick.AddListener(() =>
            {
                SetCategory(index, categoryButtons[index].transform.GetChild(0).GetChild(0).GetComponent<Image>());
            });
        }
        
        SetCategory(0, categoryButtons[0].transform.GetChild(0).GetChild(0).GetComponent<Image>());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            OpenInventory();
        //backpack.SetActive(current3DObj == null);
    }

    private IEnumerator CreateItemsStart()
    {
        foreach (var itemData in inventoryData.items)
        {
            ItemData item = GetItemData(itemData.key);
            item.cantItem = itemData.cant;
            itemsInventory.Add(CreateItem(item, continerObjects.gameObject, itemPrefab) as ItemInventoryManager);
        }

        foreach (var itemData in documentsData.posters)
        {
            PosterItemInventoryManager itemInventory = CreateItem(itemData, continerPosters.gameObject, itemPosterPrefab) as PosterItemInventoryManager;
            postersInventory.Add(itemInventory);
            itemInventory.isUnlocked = inventoryData.poster.Find(it => it.Equals(itemInventory.key)) != null;
        }

        yield return null;
        
        SetObj3D(itemsInventory.Count > 0 ? itemsInventory[0] : null);
    }

    private ItemInventory CreateItem(ItemData itemData, GameObject container, GameObject prefab)
    {
        GameObject itemObj = Instantiate(prefab, container.transform);
        ItemInventory itemManager = itemObj.GetComponent<ItemInventory>();
        itemManager.SetData(itemData.keyItem);

        return itemManager;
    }

    public void SetObj3D(ItemInventory itemManager)
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

        ItemData itemData = null;

        switch (currentCategory)
        {
            case Category.OBJECTS:
                itemData = GetItemData(itemManager.key);
                GameObject obj3D = Instantiate(itemData.prefabItem, continer3DObj.transform);
                current3DObj = obj3D;
                break;
            
            case Category.POSTERS:
                var dataPoster = GetPosterData(itemManager.key);
                dataPoster.isUnlocked = IsPosterUnlocked(itemManager.key);
                itemData = dataPoster;
                Sprite sprite = dataPoster.ValueFront;
                imageDocument.sprite = sprite;
                imageDocument.color = sprite != null ? Color.white : Color.black;
                break;
        }
        
        lastItemAbove = itemManager.gameObject;
        itemManager.selector.transform.localScale = Vector3.one;
        textDescription.SetText(itemData.descriptionItem);
    }

    public void AddObjectToInventory(string key, int cant)
    {
        Debug.Log("AddObjectToInventory");
        ItemData itemData = GetItemData(key);
        if (itemData == null)
        {
            Debug.Log("Item with key " + key + " not found in All Items Data.");
            return;
        }

        itemData.cantItem = cant;
        
        foreach (var itemInventory in itemsInventory)
        {
            if (itemData.keyItem.Equals(itemInventory.data.keyItem))
            {
                inventoryData.items.Find(it => it.key == key).cant += cant;
                itemInventory.data.cantItem += itemData.cantItem;
                SaveDataInventory();
                return;
            }
        }
        
        inventoryData.items.Add(new DataItem { key = key, cant = cant });
        itemsInventory.Add(CreateItem(itemData, continerObjects.gameObject, itemPrefab) as ItemInventoryManager);
        SaveDataInventory();
    }

    public void ShowObject(string key = "")
    {
        switch (currentCategory)
        {
            case Category.OBJECTS:
                ViewObjectController.instance.ShowObject();
                break;
            case Category.POSTERS:
                EventBus<StartDiapositiveEvent>.Raise(new StartDiapositiveEvent
                    { keyDiapositive = key });
                break;
        }
    }

    public void UnlockPoster(string keyPoster)
    {
        if (inventoryData.poster.Find(it => it == keyPoster) == null)
        {
            inventoryData.poster.Add(keyPoster);
            PosterItemInventoryManager itemInventory = postersInventory.Find(it => it.key == keyPoster);
            itemInventory.isUnlocked = true;
        }
        SaveDataInventory();
    }

    public bool IsPosterUnlocked(string keyPoster)
    {
        return inventoryData.poster.Find(it => it.Equals(keyPoster)) != null;
    }
    
    public ItemData GetItemData(string keyItem)
    {
        return allItemsData.allItems.Find(it => it.keyItem == keyItem);
    }
    
    public DocumentData GetPosterData(string keyPoster)
    {
        return documentsData.posters.Find(it => it.keyItem == keyPoster);
    }
    
    public DocumentData GetDocumentData(string keyPoster)
    {
        DocumentData documentData = documentsData.posters.Find(it => it.keyItem == keyPoster);
        if (documentData != null) return documentData;
        
        documentData = documentsData.documents.Find(it => it.keyItem == keyPoster);
        if (documentData != null) return documentData;

        return null;
    }
    
    public bool HasItem(string keyItem)
    {
        return inventoryData.items.Find(it => it.key == keyItem) != null;
    }

    public void RemoveItemFromInventory(string keyItem, int cant)
    {
        DataItem dataItem = inventoryData.items.Find(it => it.key == keyItem);
        dataItem.cant -= cant;
        if (dataItem.cant <= 0)
            inventoryData.items.Remove(dataItem);
    }

    public void SetCategory(int numCategory, Image imageCategory)
    {
        currentCategory = (Category)numCategory;

        objImage.transform.localScale = Vector3.zero;
        imageDocument.transform.localScale = Vector3.zero;

        continerObjects.transform.localScale = Vector3.zero;
        continerPosters.transform.localScale = Vector3.zero;

        foreach (var categoryButton in categoryButtons)
            categoryButton.transform.GetChild(0).GetChild(0).GetComponent<Image>().DOFade(0.6f, 0).SetUpdate(true);
        imageCategory.DOFade(1, 0).SetUpdate(true);

        GameObject finalContiner = null;

        switch (numCategory)
        {
            default:
                objImage.transform.localScale = Vector3.one;
                finalContiner = continerObjects;
                if (itemsInventory.Count > 0) SetObj3D(itemsInventory[0]);
                break;
            case 1:
                imageDocument.transform.localScale = Vector3.one;
                finalContiner = continerPosters;
                if (postersInventory.Count > 0) SetObj3D(postersInventory[0]);
                break;
        }

        if (finalContiner == null) return;
        
        finalContiner.transform.localScale = Vector3.one;

        scrollRect.content = finalContiner.GetComponent<RectTransform>();
    }

    private void SaveDataInventory()
    {
        PlayerPrefs.SetString("InventoryData", JsonUtility.ToJson(inventoryData));
    }

    private void OpenInventory()
    {
        if (PausaController.instance.isPauseShowed) return;

        isInventoryShowed = !isInventoryShowed;
        
        PausaController.instance.Pause();
        canvasInventory.alpha = isInventoryShowed ? 1 : 0;
        canvasInventory.blocksRaycasts = isInventoryShowed;
    }
}
