using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ReadingTextController : MonoBehaviour
{
    public static ReadingTextController instance;

    public Camera camera;
    
    public GameObject fame;

    public string textFront;
    public string textBack;
    public string currentText;

    public TextMeshProUGUI readingText;

    public GameObject buttonRead;

    public bool isReading = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        EventBus<SetReadingText>.Register(new EventBinding<SetReadingText>(SetTexts, gameObject));
        EventBus<HideAllScenes>.Register(new EventBinding<HideAllScenes>(RestartVariables, gameObject));
    }

    private void OnDestroy()
    {
        EventBus<SetReadingText>.Deregister(new EventBinding<SetReadingText>(SetTexts, gameObject));
        EventBus<HideAllScenes>.Deregister(new EventBinding<HideAllScenes>(RestartVariables, gameObject));
    }

    private void SetTexts(SetReadingText setReadingText)
    {
        textFront = setReadingText.textFront;
        textBack = setReadingText.textBack;
    }
    
    private void Update()
    {
        currentText = fame.transform.localRotation.y > -0.7f && fame.transform.localRotation.y < 0.7f
            ? textFront
            : textBack;
        
        buttonRead.SetActive(!currentText.Equals("") && !isReading);
    }

    public void ShowText()
    {
        isReading = true;

        readingText.text = currentText;

        readingText.transform.parent.localScale = Vector3.one;
    }

    private void RestartVariables()
    {
        textFront = "";
        textBack = "";
    }

    public void CloseText()
    {
        isReading = false;
        readingText.transform.parent.localScale = Vector3.zero;
    }
}
