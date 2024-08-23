using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PanelDialogoController : MonoBehaviour
{
    public static PanelDialogoController instance;
    
    public GameObject panelPlayer;
    public GameObject panelNPC;

    private GameObject lastPanelShow;
    
    private Image imagenNPC;

    private string lastHablante = "";

    public bool animando = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        imagenNPC = GameObject.Find("ImagenNPC").GetComponent<Image>();
        animationImageEnter();
    }

    public async void mostrarPanel(string hablante, Action callback1 = null, Action callback2 = null)
    {
        if (animando) return;
        
        animando = true;
        GameObject objToMove = hablante.Equals("Player") ? panelPlayer : panelNPC;

        transform.localScale = Vector3.one;
        
        if (!hablante.Equals("Player")) setImagenPanel(hablante);

        panelPlayer.transform.DOKill();
        panelNPC.transform.DOKill();
        
        if (lastPanelShow != null && !lastHablante.Equals(hablante)) 
            await lastPanelShow.transform.DOLocalMoveY(-475, 0.5f).SetEase(Ease.InBack).AsyncWaitForCompletion();
        
        callback1?.Invoke();

        if (!lastHablante.Equals(hablante)) 
            await objToMove.transform.DOLocalMoveY(-175, 0.5f).SetEase(Ease.OutBack).AsyncWaitForCompletion();

        callback2?.Invoke();

        lastPanelShow = objToMove;
        lastHablante = hablante;
        animando = false;
    }

    public async void esconderPanel(Action callback = null)
    {
        if (animando) return;
        
        animando = true;
        
        lastHablante = "";
        panelPlayer.transform.DOLocalMoveY(-475, 0.5f).SetEase(Ease.InBack);
        await panelNPC.transform.DOLocalMoveY(-475, 0.5f).SetEase(Ease.InBack).AsyncWaitForCompletion();
        
        callback?.Invoke();
        
        transform.localScale = Vector3.zero;
        animando = false;
    }

    public void setImagenPanel(string hablante)
    {
        imagenNPC.sprite = UnityEngine.Resources.Load<Sprite>("Sprites/ImagenesDialogo/"+hablante);
    }

    private void animationImageEnter()
    {
        List<GameObject> imagesEnter = new List<GameObject>
        {
            transform.GetChild(0).GetChild(3).gameObject,
            transform.GetChild(1).GetChild(3).gameObject
        };

        foreach (var enterObj in imagesEnter)
        {
            Sequence sequenceAnimation = DOTween.Sequence();
            sequenceAnimation.SetLoops(-1);
            float yPosition = enterObj.transform.localPosition.y;
            sequenceAnimation.Append(enterObj.transform.DOLocalMoveY(yPosition+ 5, 1));
            sequenceAnimation.Append(enterObj.transform.DOLocalMoveY(yPosition, 1));
        }
    }
}
