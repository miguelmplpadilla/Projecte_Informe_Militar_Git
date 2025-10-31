using System.Collections;
using UnityEngine;

namespace Resources.Scripts.Objects
{
    public class Poster : ObjPickUp
    {
        public DiapositiveNode.DataDiapositive dataDiapositive;
        
        protected override IEnumerator Inter()
        {
            yield return new WaitForSeconds(2);
            
            EventBus<StartDiapositive>.Raise(new StartDiapositive
            {
                imageDiapositiveFront = dataDiapositive.frontES,
                imageDiapositiveBack = dataDiapositive.backES,
                backgroundDiapositive = dataDiapositive.backgroundDiapositive,
                descriptionFront = dataDiapositive.descriptionFront,
                descriptionBack = dataDiapositive.descriptionBack,
                textFront = dataDiapositive.textFront,
                textBack = dataDiapositive.textBack
            });
            
            Time.timeScale = 0;
            Destroy(gameObject);
        }
    }
}