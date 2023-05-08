using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Resources.Scripts;
using UnityEngine;

public class JSONConverter : MonoBehaviour
{
    [SerializeField] private TextAsset jsonDialogos;

    public Root textos;
    public Dictionary<string, Passage> dialogos = new Dictionary<string, Passage>();

    private void Awake()
    {
        string countryJsonStr = jsonDialogos.text;

        textos = JsonConvert.DeserializeObject<Root>(countryJsonStr);

        for (int i = 0; i < textos.passages.Count; i++)
        {
            dialogos.Add(textos.passages[i].name, textos.passages[i]);
        }
    }
}
