using System;
using System.Collections;
using System.Collections.Generic;
using Resources.Scripts;
using UnityEngine;

public class BotonOpcionController : MonoBehaviour
{
    public Passage passage;
    private DialogeController _dialogeController;

    private void Start()
    {
        _dialogeController = GameObject.Find("TextoNpc").GetComponent<DialogeController>();
    }

    public void responder()
    {
        _dialogeController.iniciarMostrarTexto(passage);
    }
}
