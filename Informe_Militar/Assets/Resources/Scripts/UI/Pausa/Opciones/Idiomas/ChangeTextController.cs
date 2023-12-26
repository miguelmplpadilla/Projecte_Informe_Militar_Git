using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ChangeTextController : MonoBehaviour
{
    public string id;
    public TextType textType;
    
    private TextMeshProUGUI textMesh;

    public enum TextType
    {
        Button, UI, UIInventory
    }

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void changeText(string newId = "")
    {
        if (!newId.Equals("")) id = newId;

        string text = JSONConverter.getText(textType.ToString(), id);
        textMesh.text = text;
    }
}
