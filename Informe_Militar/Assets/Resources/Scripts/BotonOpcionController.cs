using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonOpcionController : MonoBehaviour
{
    public string id;

    private DialogeController dialogeController;

    private void Start()
    {
        dialogeController = GameObject.Find("TextoPrueba").GetComponent<DialogeController>();
    }

    public void responder()
    {
        string texto = dialogeController.montarString(dialogeController.jsonConverter.dialogos[id].text);
        
        dialogeController.iniciarMostrarTexto(texto);
        
        dialogeController.createOptionsArroundPoint(dialogeController.jsonConverter.dialogos[id]);
    }
}
