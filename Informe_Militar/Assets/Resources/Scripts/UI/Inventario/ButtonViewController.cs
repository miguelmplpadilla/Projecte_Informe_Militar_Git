using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Resources.Scripts.UI.Inventario
{
    public class ButtonViewController : MonoBehaviour
    {
        public string id;

        public async void showDocument()
        {
            GameObject imageDocument = GameObject.Find("ImageDocument");
        
            Sprite spriteDocument = UnityEngine.Resources.Load<Sprite>("Sprites/Documents/" + id);

            imageDocument.GetComponent<Image>().sprite = spriteDocument;

            await imageDocument.transform.parent.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetUpdate(true)
                .AsyncWaitForCompletion();
            imageDocument.transform.parent.DOKill();
        }

        public async void showFoto()
        {
            GameObject imageDocument = GameObject.Find("ImageFoto");
        
            Sprite spriteDocument = UnityEngine.Resources.Load<Sprite>("Sprites/Camera/Fotografias/" + id);

            imageDocument.GetComponent<Image>().sprite = spriteDocument;

            await imageDocument.transform.parent.parent.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetUpdate(true)
                .AsyncWaitForCompletion();
            imageDocument.transform.parent.DOKill();
        }
    }
}