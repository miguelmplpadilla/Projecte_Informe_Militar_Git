using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Resources.Scripts.Objects
{
    public class Poster : ObjPickUp
    {
        public Image imagePoster;

        protected override void Start()
        {
            base.Start();
            var data = InventoryManager.instance.GetDocumentData(key);
            data.isUnlocked = true;
            imagePoster.sprite = data.ValueFront;
        }

        protected override IEnumerator Inter()
        {
            yield return new WaitForSeconds(2);
            
            InventoryManager.instance.UnlockPoster(key);
            EventBus<StartDiapositiveEvent>.Raise(new StartDiapositiveEvent
            { keyDiapositive = key });
            
            Time.timeScale = 0;
            Destroy(gameObject);
        }
    }
}