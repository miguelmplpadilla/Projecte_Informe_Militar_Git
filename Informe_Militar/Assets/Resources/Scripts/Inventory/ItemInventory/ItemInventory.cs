using Resources.Scripts.Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Resources.Scripts.Inventory.ItemInventory
{
    public class ItemInventory : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string key;
        
        public LocalizableController text;

        public GameObject selector;
        public Button selectItem;
        
        private void Start()
        {
            selector.transform.localScale = Vector3.zero;
        
            EventBus<HideSelectorEvent>.Register(new EventBinding<HideSelectorEvent>(HideSelector, gameObject));
        
            selectItem.onClick.AddListener(() =>
            { 
                ShowObject();
            });
        }
        
        private void OnDestroy()
        {
            EventBus<HideSelectorEvent>.Deregister(new EventBinding<HideSelectorEvent>(HideSelector, gameObject));
        }
        
        protected virtual void Update()
        {
        }
        
        private void HideSelector(HideSelectorEvent e)
        {
            if (e.objNotHide != null && e.objNotHide.Equals(gameObject)) return;
            selector.transform.localScale = Vector3.zero;
        }
        
        protected virtual void ShowObject()
        {
            InventoryManager.instance.ShowObject(key);
        }
        
        public virtual void SetData(string itemKey)
        {
            key = itemKey;
            SetText();
        }

        protected virtual void SetText()
        {
        }
        
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            InventoryManager.instance.SetObj3D(this);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            InventoryManager.instance.SetObj3D(null);
        }
    }
}