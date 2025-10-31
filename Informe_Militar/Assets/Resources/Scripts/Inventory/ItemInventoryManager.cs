using System;
using Resources.Scripts.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInventoryManager : MonoBehaviour, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ItemData data;

    public TextMeshProUGUI text;

    public GameObject selector;

    private void Start()
    {
        selector.transform.localScale = Vector3.zero;
        
        EventBus<HideSelectorEvent>.Register(new EventBinding<HideSelectorEvent>(HideSelector, gameObject));
    }

    private void OnDestroy()
    {
        EventBus<HideSelectorEvent>.Deregister(new EventBinding<HideSelectorEvent>(HideSelector, gameObject));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Item Clicked");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InventoryManager.instance.SetObj3D(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.instance.SetObj3D(null);
    }

    public void SetData(ItemData itemData)
    {
        data = new ItemData(itemData);

        text.text = itemData.nameItem.Value;
    }
    
    private void HideSelector(HideSelectorEvent e)
    {
        if (e.objNotHide != null && e.objNotHide.Equals(gameObject)) return;
        selector.transform.localScale = Vector3.zero;
    }
}

public class HideSelectorEvent : IEvent
{
    public GameObject objNotHide;
}