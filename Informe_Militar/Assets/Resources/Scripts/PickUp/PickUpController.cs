using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PickUpController : MonoBehaviour
{
    public string functionExecute = "";
    public string id = "";
    
    public void interEnter(PlayerModel model)
    {
    }

    public void inter(PlayerModel model)
    {
        if (!functionExecute.Equals("")) 
            gameObject.SendMessage(functionExecute, id);
    }

    public void interExit(PlayerModel model)
    {
    }

    public async void pickUpDocument(string documentId)
    {
        GameObject imageDocument = GameObject.Find("ImageDocument");
        
        Sprite spriteDocument = UnityEngine.Resources.Load<Sprite>("Sprites/Documents/" + documentId);

        imageDocument.GetComponent<Image>().sprite = spriteDocument;

        Animator animatorPLayer = GameObject.Find("Player").GetComponent<Animator>();
        animatorPLayer.SetTrigger("agachar");
        
        await Task.Delay(550);

        await imageDocument.transform.parent.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).AsyncWaitForCompletion();

        imageDocument.transform.parent.DOKill();
        Destroy(gameObject);
    }
}
