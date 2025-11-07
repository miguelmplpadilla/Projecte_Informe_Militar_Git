using System.Collections;
using UnityEngine;

namespace Resources.Scripts.Objects
{
    public class Poster : ObjPickUp
    {
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