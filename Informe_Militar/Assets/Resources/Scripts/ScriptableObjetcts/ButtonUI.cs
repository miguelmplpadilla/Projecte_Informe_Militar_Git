using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ButtonsUI", menuName = "ScriptableObjects/ButtonUI")]
public class ButtonUI : ScriptableObject
{
    public List<SpriteButton> Buttons = new List<SpriteButton>();
}

[Serializable]
public class SpriteButton
{
    public KeyCode keycode;
    public Sprite keyboardButton;
    public Sprite controllerPcButtonSprite;
    public Sprite nintendoButtonSprite;
}