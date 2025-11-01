using System.Collections;
using UnityEngine;

namespace Resources.Scripts.Objects
{
    public class Poster : ObjPickUp
    {
        public string keyDocument;
        
        protected override IEnumerator Inter()
        {
            yield return new WaitForSeconds(2);
            
            EventBus<StartDiapositiveEvent>.Raise(new StartDiapositiveEvent
            { keyDiapositive = keyDocument });
            
            Time.timeScale = 0;
            Destroy(gameObject);
        }
    }
}