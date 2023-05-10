using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Resources.Scripts;
using TMPro;
using UnityEngine;

public class DialogeController : MonoBehaviour
{
    public JSONConverter jsonConverter;
    public TextAnimationController textAnimationController;
    
    public GameObject botonPrefab;
    private GameObject botones;
    
    public float distanciaOpciones = 10;

    void Start()
    {
        jsonConverter = GameObject.Find("TextoPrueba").GetComponent<JSONConverter>();
        textAnimationController = GameObject.Find("TextoPrueba").GetComponent<TextAnimationController>();

        string textoMostrar = montarString(jsonConverter.textos.passages[0].text);
        
        botones = GameObject.Find("ContentBotones");
        
        createOptionsArroundPoint(jsonConverter.textos.passages[0]);
        
        iniciarMostrarTexto(textoMostrar);
    }

    public void iniciarMostrarTexto(string text)
    {
        textAnimationController.iniciarMostrarTexto(text);
    }

    public string montarString(string stringMontar)
    {
        string stringReturn = "";
        
        for (int i = 0; i < stringMontar.Length; i++)
        {
            if (!stringMontar[i].Equals('['))
            {
                stringReturn += stringMontar[i];
            }
            else
            {
                break;
            }
        }

        return stringReturn;
    }
    
    public void createOptionsArroundPoint(Passage passage)
    {

        GameObject[] botonesEnEscena = GameObject.FindGameObjectsWithTag("BotonOpcion");

        for (int i = 0; i < botonesEnEscena.Length; i++)
        {
            Destroy(botonesEnEscena[i]);
        }

        if (passage.links != null)
        {
            for (int i = 0; i < passage.links.Count; i++)
            {
                float angle = i * Mathf.PI*2f / passage.links.Count;

                Vector3 newPos = new Vector3(botones.transform.position.x + Mathf.Cos(angle)*distanciaOpciones, botones.transform.position.y + Mathf.Sin(angle)*distanciaOpciones);
                GameObject boton = Instantiate(botonPrefab, botones.transform);

                boton.GetComponentInChildren<TextMeshProUGUI>().text = jsonConverter.dialogos[passage.links[i].name].name;

                boton.GetComponent<BotonOpcionController>().id = passage.links[i].name;
            
                
            }
        }
    }  
}
