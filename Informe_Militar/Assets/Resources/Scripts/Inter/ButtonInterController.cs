using DG.Tweening;
using UnityEngine;

public class ButtonInterController : MonoBehaviour
{
    private Vector3 originalScale;
    private SpriteRenderer spriteRenderer;

    public KeyCode key = KeyCode.F;
    
    public void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
        transform.localScale = Vector3.zero;

        ChangeSprite();
    }

    public void mostrar()
    {
        transform.DOScale(originalScale, 0.1f);
    }

    public void esconder()
    {
        transform.DOScale(Vector3.zero, 0.1f);
    }

    public void ChangeSprite()
    {
        spriteRenderer.sprite = ButtonUIManager.GetSprite(key);
    }

    public void SetKey(KeyCode keyCode)
    {
        key = keyCode;
        ChangeSprite(); ;
    }
}
