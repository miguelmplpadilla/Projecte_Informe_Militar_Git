using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Resources.Scripts;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DialogeController : MonoBehaviour
{
    public JSONConverter jsonConverter;
    public TextAnimationController textAnimationController;

    public GameObject botonPrefab;
    private GameObject botones;

    public float distanciaOpciones = 10;

    private Passage siguietenPassage;

    private bool multiOpcion = false;

    void Start()
    {
        jsonConverter = GameObject.Find("TextoPrueba").GetComponent<JSONConverter>();
        textAnimationController = GameObject.Find("TextoPrueba").GetComponent<TextAnimationController>();

        botones = GameObject.Find("ContentBotones");

        iniciarMostrarTexto(jsonConverter.textos.passages[0]);
    }

    private void Update()
    {
        if (!multiOpcion)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                iniciarMostrarTexto(siguietenPassage);
            }
        }
    }

    public void iniciarMostrarTexto(Passage text)
    {
        if (text != null)
        {
            string textoMostrar = montarString(text.text);

            textAnimationController.iniciarMostrarTexto(textoMostrar);

            createOptionsArroundPoint(text);

            ejecutarTags(text);
        }
        else
        {
            // Esconder dialogos
        }
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
            if (passage.links.Count > 1)
            {
                multiOpcion = true;
                for (int i = 0; i < passage.links.Count; i++)
                {
                    GameObject boton = Instantiate(botonPrefab, botones.transform);

                    boton.GetComponentInChildren<TextMeshProUGUI>().text =
                        jsonConverter.dialogos[passage.links[i].name].name;

                    boton.GetComponent<BotonOpcionController>().id = passage.links[i].name;
                }
            }
            else
            {
                multiOpcion = false;
                siguietenPassage = jsonConverter.dialogos[passage.links[0].name];
            }
        }
        else
        {
            multiOpcion = false;
            siguietenPassage = null;
        }
    }

    public void ejecutarTags(Passage pasage)
    {
        if (pasage.tags != null)
        {
            for (int i = 0; i < pasage.tags.Count; i++)
            {
                string[] splitTags = Regex.Split(pasage.tags[i], ">");
                if (splitTags[0].Equals("Funcion"))
                {
                    gameObject.BroadcastMessage(splitTags[1]);
                }
            }
        }
    }

    public void mostrarTexto()
    {
        Debug.Log("Texto mostrado");
    }
}