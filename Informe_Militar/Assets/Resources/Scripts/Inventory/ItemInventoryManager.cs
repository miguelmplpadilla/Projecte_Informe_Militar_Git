using Resources.Scripts.Inventory;
using Resources.Scripts.Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemInventoryManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemData data;

    public LocalizableController text;

    public GameObject selector;
    public Button selectItem;

    private void Start()
    {
        selector.transform.localScale = Vector3.zero;
        
        EventBus<HideSelectorEvent>.Register(new EventBinding<HideSelectorEvent>(HideSelector, gameObject));
        
        selectItem.onClick.AddListener(() =>
        {
            ViewObjectController.instance.ShowObject(data);
        });
    }

    private void OnDestroy()
    {
        EventBus<HideSelectorEvent>.Deregister(new EventBinding<HideSelectorEvent>(HideSelector, gameObject));
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

        text.SetText(itemData.nameItem);
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