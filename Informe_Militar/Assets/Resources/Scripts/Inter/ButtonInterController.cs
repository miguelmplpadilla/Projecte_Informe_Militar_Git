using DG.Tweening;
using UnityEngine;

public class ButtonInterController : MonoBehaviour
{
    private Vector3 originalScale;
    
    public void Start()
    {
        originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    public void mostrar(string textInter)
    {
        transform.DOScale(originalScale, 0.1f);
    }

    public void esconder()
    {
        transform.DOScale(Vector3.zero, 0.1f);
    }
}
