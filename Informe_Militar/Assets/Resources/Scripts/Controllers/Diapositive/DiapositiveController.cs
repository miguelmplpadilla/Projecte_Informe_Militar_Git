using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DiapositiveController : MonoBehaviour
{
    public Image imageDiapositive;
    public Image backgroundDiapositive;
    
    void Start()
    {
        EventBus<StartDiapositive>.Register(new EventBinding<StartDiapositive>(StartDiapositive));
    }

    private void OnDestroy()
    {
        EventBus<StartDiapositive>.Deregister(new EventBinding<StartDiapositive>(StartDiapositive));
    }

    private void StartDiapositive(StartDiapositive diapositive)
    {
        imageDiapositive.sprite = diapositive.imageDiapositive;
        backgroundDiapositive.sprite = diapositive.backgroundDiapositive;

        backgroundDiapositive.DOFade(diapositive.backgroundDiapositive == null ? 0.7f : 1, 0);
    }

    public void CloseDiapisitive()
    {
        
    }
}
