using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PickUpController : MonoBehaviour
{
    private InventarioController inventarioController;

    private PausaController pausaController;

    public UnityEvent function;

    private PlayerModel playerModel;

    private void Start()
    {
        pausaController = GameObject.Find("PanelPausa").GetComponent<PausaController>();
        inventarioController = GameObject.Find("PanelInventario").GetComponent<InventarioController>();
    }

    public void interEnter(PlayerModel model)
    {
        playerModel = model;
    }

    public void inter(PlayerModel model)
    {
        playerModel = model;

        if (function.GetPersistentEventCount() > 0) {
            function.Invoke();
            return;
        }

        Debug.Log("No functions in list");
        playerModel.canInter = true;
        playerModel.mov = true;
    }

    public void interExit(PlayerModel model)
    {
    }

    public void PickUpObj(string objId)
    {
        Debug.Log("Add to inventory " + objId);

        playerModel.canInter = true;
        playerModel.mov = true;

        Destroy(gameObject);
    }

    public async void PickUpDocument(string documentId)
    {
        inventarioController.unlockDocument(documentId);
        
        GameObject imageDocument = GameObject.Find("ImageDocument");
        
        Sprite spriteDocument = UnityEngine.Resources.Load<Sprite>("Sprites/Documents/" + documentId);

        imageDocument.GetComponent<Image>().sprite = spriteDocument;

        Animator animatorPLayer = GameObject.Find("Player").GetComponent<Animator>();
        animatorPLayer.SetTrigger("agachar");
        
        await Task.Delay(550);

        pausaController.pause();

        await imageDocument.transform.parent.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetUpdate(true).AsyncWaitForCompletion();
        imageDocument.transform.parent.DOKill();

        Destroy(gameObject);
    }
}
