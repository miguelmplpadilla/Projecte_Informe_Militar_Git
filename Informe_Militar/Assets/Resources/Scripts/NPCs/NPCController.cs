using System.Collections;
using System.Collections.Generic;
using Resources.Scripts;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    private Story _textos;
    private Dictionary<string, Passage> dialogos = new Dictionary<string, Passage>();

    private GameObject _player;

    public string idJson = "HistoriaPrueba";
    private DialogeController _dialogeController;
    
    [Header("Deliver Letter Method")]
    public List<string> cardsId = new List<string>();
    
    void Start()
    {
        _player = GameObject.Find("Player");
        _dialogeController = GameObject.Find("TextoNpc").GetComponent<DialogeController>();
        
        _textos = JSONConverter.parseJson(idJson);
        
        foreach (Passage passage in _textos.passages)
        {
            dialogos.Add(passage.name, passage);
        }
    }
    
    public void interEnter(PlayerModel model)
    {
    }

    public void inter(PlayerModel model)
    {
        _dialogeController.startDialoge(_textos.passages[0], dialogos, gameObject, _textos);
    }

    public void interExit(PlayerModel model)
    {
    }

    public void deliverLetter()
    {
        foreach (var idCard in cardsId)
            _player.BroadcastMessage("deleteCardById", idCard);
    }
}
