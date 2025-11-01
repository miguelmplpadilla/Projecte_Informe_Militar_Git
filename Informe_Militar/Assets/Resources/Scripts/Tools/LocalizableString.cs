using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

[Serializable]
public class LocalizableString
{
    public string Value
    {
        get
        {
            var locale = LocalizationSettings.SelectedLocale;

            switch (locale.Identifier.Code)
            {
                case "es-ES":
                    return valueSpanish;
                case "en-US":
                    return valueEnglish;
                default:
                    return valueEnglish;
            }
        }
    }

    public LocalizableString(string valueSpanish, string valueEnglish)
    {
        this.valueSpanish = valueSpanish;
        this.valueEnglish = valueEnglish;
    }

    [SerializeField, TextArea(3, 10)] private string valueSpanish;
    [SerializeField, TextArea(3, 10)] private string valueEnglish;
}