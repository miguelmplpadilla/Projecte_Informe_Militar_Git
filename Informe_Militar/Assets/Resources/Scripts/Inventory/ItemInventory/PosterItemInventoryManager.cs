using TMPro;

namespace Resources.Scripts.Inventory.ItemInventory
{
    public class PosterItemInventoryManager : ItemInventory
    {
        public bool isUnlocked = false;

        protected override void Update()
        {
            text.text.fontStyle = !isUnlocked ? FontStyles.Strikethrough : FontStyles.Normal;
        }

        protected override void ShowObject()
        {
            if (!isUnlocked) return;
            base.ShowObject();
        }

        protected override void SetText()
        {
            text.SetText(InventoryManager.instance.GetPosterData(key).nameItem);
        }
    }
}