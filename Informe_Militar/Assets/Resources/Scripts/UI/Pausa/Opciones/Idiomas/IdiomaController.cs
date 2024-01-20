using System;
using System.Collections.Generic;
using DG.Tweening;
using Resources.Scripts.UI.Idiomas;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class IdiomaController : MonoBehaviour
{
    private TextMeshProUGUI textButtonIdioma;

    private NavigationController navigationController;

    public int indexIdioma = 0;
    public List<Idioma> idiomas;
    [NonSerialized] public Dictionary<string, Idioma> dictionaryIdiomas = new Dictionary<string, Idioma>();

    public string actualLanguage = "";

    private UIInput uiInput;

    private void Awake()
    {
        uiInput = new UIInput();

        uiInput.Navigation.Horizontal.performed += ChangeIdiomController;

        foreach (var idioma in idiomas)
        {
            dictionaryIdiomas.Add(idioma.id, idioma);
        }
    }

    private void Start()
    {
        navigationController = GameObject.Find("NavigationManager").GetComponent<NavigationController>();
        textButtonIdioma = GameObject.Find("TextIdioma").GetComponent<TextMeshProUGUI>();

        actualLanguage = getLanguage();

        changeTextLanguage(dictionaryIdiomas[actualLanguage].name);
        changeAllTexts();
    }

    private void OnEnable()
    {
        uiInput.Enable();
    }

    private void OnDisable()
    {
        uiInput.Disable();
    }

    private void ChangeIdiomController(InputAction.CallbackContext context)
    {
        if (navigationController.buttonSelected == null || !navigationController.buttonSelected.name.Equals("OpcionIdiomas")) return;
        changeIdiom(context.ReadValue<float>() > 0 ? 1 : -1);
    }

    public void changeIdiom(int sumIndex)
    {
        indexIdioma += sumIndex;

        if (indexIdioma < 0)
            indexIdioma = idiomas.Count - 1;
        else if (indexIdioma >= idiomas.Count)
            indexIdioma = 0;

        actualLanguage = idiomas[indexIdioma].id;
        
        PlayerPrefs.SetString("idioma", actualLanguage);
        
        changeTextLanguage(idiomas[indexIdioma].name);
        
        changeAllTexts();
    }

    private void changeTextLanguage(string idioma)
    {
        textButtonIdioma.text = idioma;
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

        GameObject.Find("Diary").GetComponent<DiaryController>().ChangeIdiomDiary();
    }
}
