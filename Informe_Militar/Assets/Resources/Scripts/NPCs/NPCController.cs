using System.Collections;
using System.Collections.Generic;
using Resources.Scripts;
using UnityEngine;

public class NPCController : MonoBehaviour, InterBaseInterface
{
    private Story _textos;
    private Dictionary<string, Passage> dialogos = new Dictionary<string, Passage>();

    private GameObject _player;

    public string idJson = "HistoriaPrueba";
    private DialogeController _dialogeController;

    private Animator _animator;

    public bool scaleRevert = false;
    
    //[Header("Deliver Letter Method")]
    
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        
        _player = GameObject.Find("Player");
        _dialogeController = GameObject.Find("TextoNpc").GetComponent<DialogeController>();
    }
    
    public void interEnter(PlayerModel model)
    {
    }

    public void inter(PlayerModel model)
    {
        _textos = JSONConverter.parseJson(idJson);
        
        dialogos.Clear();
        
        foreach (Passage passage in _textos.passages)
        {
            dialogos.Add(passage.name, passage);
        }

        int xScale = model.transform.position.x > transform.position.x ? 1 : -1;
        if (scaleRevert) xScale = model.transform.position.x > transform.position.x ? -1 : 1;
        
        transform.GetChild(0).localScale = new Vector3(xScale, 1, 1);
        _dialogeController.startDialoge(_textos.passages[0], dialogos, gameObject, _textos);
        setIdleAnimation();
    }

    public void interExit(PlayerModel model)
    {
    }

    public void changeAnimation1()
    {
        CancelInvoke("setIdleAnimation");
        _animator.SetTrigger("dance1");
        Invoke("setIdleAnimation", 10);
    }
    
    public void changeAnimation2()
    {
        CancelInvoke("setIdleAnimation");
        _animator.SetTrigger("dance2");
        Invoke("setIdleAnimation", 10);
    }
    
    public void changeAnimation3()
    {
        CancelInvoke("setIdleAnimation");
        _animator.SetTrigger("dance3");
        Invoke("setIdleAnimation", 10);
    }

    private void setIdleAnimation()
    {
        _animator.SetTrigger("idle");
    }
}
