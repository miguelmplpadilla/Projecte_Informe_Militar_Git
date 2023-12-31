using UnityEngine;
using UnityEngine.UI;

public class UIButtonChanger : MonoBehaviour
{
    public Sprite spiriteKeyboard;
    public Sprite spriteControllerPC;
    public Sprite spriteNintendoSwitch;

    private Image image;

    public Vector3 originalScale;


    private void Start()
    {
        originalScale = transform.localScale;
        image = GetComponent<Image>();

        image.sprite = GetSprite();
    }

    private void LateUpdate()
    {
        transform.localScale = originalScale;
        if (Input.GetJoystickNames().Length > 0)
            transform.localScale = Vector3.one;

        if (image.sprite == null)
            transform.localScale = Vector2.zero;

        image.sprite = GetSprite();
    }

    private Sprite GetSprite()
    {
        Sprite spriteButtonUI = spiriteKeyboard;

        if (Input.GetJoystickNames().Length > 0)
            spriteButtonUI = spriteControllerPC;

        if (Application.platform == RuntimePlatform.Switch)
            spriteButtonUI = spriteNintendoSwitch;

        return spriteButtonUI;
    }
}
