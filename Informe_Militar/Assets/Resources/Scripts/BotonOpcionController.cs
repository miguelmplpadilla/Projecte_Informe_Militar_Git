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
        dialogeController.iniciarMostrarTexto(dialogeController.jsonConverter.dialogos[id]);
    }
}
