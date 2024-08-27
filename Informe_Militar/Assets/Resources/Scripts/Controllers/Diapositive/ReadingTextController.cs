using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ReadingTextController : MonoBehaviour
{
    public static ReadingTextController instance;
    
    public GameObject objToRead;

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
        EventBus<SetReadingText>.Register(new EventBinding<SetReadingText>(SetTexts));
        EventBus<HideAllScenes>.Register(new EventBinding<HideAllScenes>(RestartVariables));
    }

    private void OnDestroy()
    {
        EventBus<SetReadingText>.Deregister(new EventBinding<SetReadingText>(SetTexts));
        EventBus<HideAllScenes>.Deregister(new EventBinding<HideAllScenes>(RestartVariables));
    }

    private void SetTexts(SetReadingText setReadingText)
    {
        textFront = setReadingText.textFront;
        textBack = setReadingText.textBack;
    }
    
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2));
        
        List<RaycastHit> hitsDrag = Physics.RaycastAll(ray).ToList();
        hitsDrag.RemoveAll(p => p.collider.name == "PanelFrame");
        hitsDrag.Sort((x, y) => x.distance.CompareTo(y.distance));

        objToRead = hitsDrag.Count > 0 ? hitsDrag[0].collider.gameObject : null;
        
        if (objToRead == null) return;

        currentText = objToRead.name.Equals("Front") ? textFront : textBack;
        
        buttonRead.SetActive(!currentText.Equals("") && !isReading);
    }

    public void ShowText()
    {
        if (objToRead == null) return;

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
