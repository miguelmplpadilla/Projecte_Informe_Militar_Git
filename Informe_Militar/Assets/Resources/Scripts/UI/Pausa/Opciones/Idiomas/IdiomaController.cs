using System;
using System.Collections.Generic;
using DG.Tweening;
using Resources.Scripts.UI.Idiomas;
using TMPro;
using UnityEngine;

public class IdiomaController : MonoBehaviour
{
    private TextMeshProUGUI textButtonIdioma;
    private GameObject imageArrow;
    
    private RectTransform _rectTransformDropdown;
    private Vector3 originalSizeDelta;

    public List<Idioma> idiomas;
    [NonSerialized] public Dictionary<string, Idioma> dictionaryIdiomas = new Dictionary<string, Idioma>();

    public string actualLanguage = "";

    private bool dropdownOpened = false;
    private bool openingDropdown = false;

    private void Awake()
    {
        foreach (var idioma in idiomas)
        {
            dictionaryIdiomas.Add(idioma.id, idioma);
        }
    }

    private void Start()
    {
        imageArrow = GameObject.Find("ImageArrow");
        _rectTransformDropdown = GameObject.Find("Content").GetComponent<RectTransform>();
        originalSizeDelta = _rectTransformDropdown.sizeDelta;
        _rectTransformDropdown.sizeDelta = new Vector2(originalSizeDelta.x, 0);
        
        textButtonIdioma = GameObject.Find("TextIdioma").GetComponent<TextMeshProUGUI>();

        actualLanguage = getLanguage();

        changeTextLanguage(actualLanguage);
        changeAllTexts();
    }

    public async void openCloseDropdownLanguage()
    {
        openingDropdown = true;
        await _rectTransformDropdown
            .DOSizeDelta(!dropdownOpened ? originalSizeDelta : new Vector2(originalSizeDelta.x, 0), 0.5f)
            .SetEase(dropdownOpened ? Ease.InBack : Ease.OutBack).SetUpdate(true).AsyncWaitForCompletion();
        
        dropdownOpened = !dropdownOpened;
        imageArrow.transform.localScale = new Vector3(1,dropdownOpened ? -1 : 1, 1);
        openingDropdown = false;
    }

    public void changeIdiom(string idioma)
    {
        if (openingDropdown || idioma.Equals(actualLanguage)) return;
        
        openCloseDropdownLanguage();

        actualLanguage = idioma;
        
        PlayerPrefs.SetString("idioma", idioma);
        
        changeTextLanguage(idioma);
        
        changeAllTexts();
    }

    private void changeTextLanguage(string idioma)
    {
        textButtonIdioma.text = dictionaryIdiomas[idioma].name;
    }

    public static string getLanguage()
    {
        return PlayerPrefs.HasKey("idioma") ? PlayerPrefs.GetString("idioma") : "ES";
    }

    private void changeAllTexts()
    {
        ChangeTextController[] allChangeTexts = FindObjectsOfType<ChangeTextController>();

        foreach (var changeTextObj in allChangeTexts)
        {
            changeTextObj.changeText();
        }
    }
}
