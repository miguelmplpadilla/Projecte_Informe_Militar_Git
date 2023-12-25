using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class ButtonUIManager : MonoBehaviour
{
    public static Sprite GetSprite(KeyCode keycode)
    {
        Dictionary<KeyCode, SpriteButton> buttonDic =
            new Dictionary<KeyCode, SpriteButton>();

        ButtonUI buttons = 
            AssetDatabase.LoadAssetAtPath<ButtonUI>("Assets/Resources/Scripts/ScriptableObjetcts/Objects/ButtonUI.asset");

        foreach (var button in buttons.Buttons)
            buttonDic.Add(button.keycode, button);

        Sprite spriteButtonUI = GetSpriteUI(buttonDic, keycode);

        if (Input.GetJoystickNames().Length > 0)
            spriteButtonUI = buttonDic[keycode].controllerPcButtonSprite;

        if (Application.platform == RuntimePlatform.Switch)
            spriteButtonUI = buttonDic[keycode].nintendoButtonSprite;

        return spriteButtonUI;
    }

    private static Sprite GetSpriteUI(Dictionary<KeyCode, SpriteButton> buttonDic, KeyCode keycode)
    {
        Sprite spriteButtonUI = buttonDic[keycode].keyboardButton;

        if (Input.GetJoystickNames().Length > 0)
            spriteButtonUI = buttonDic[keycode].controllerPcButtonSprite;

        if (Application.platform == RuntimePlatform.Switch)
            spriteButtonUI = buttonDic[keycode].nintendoButtonSprite;

        return spriteButtonUI;
    }
}
