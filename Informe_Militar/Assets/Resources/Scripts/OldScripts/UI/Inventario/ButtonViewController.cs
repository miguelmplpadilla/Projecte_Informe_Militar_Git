using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DG.Tweening;
using TMPro;
using UnityEditor;
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

            imageDocument.transform.parent.Find("CloseButton").GetComponent<DocumentPanelController>().panelShowed = true;

            Sprite spriteDocument = UnityEngine.Resources.Load<Sprite>("Sprites/Documents/" + id);

            imageDocument.GetComponent<Image>().sprite = spriteDocument;

            await imageDocument.transform.parent.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetUpdate(true)
                .AsyncWaitForCompletion();
            imageDocument.transform.parent.DOKill();
        }

        public async void showFoto()
        {
            GameObject panelFotos = GameObject.Find("PanelFotos");

            panelFotos.transform.Find("CloseButton").GetComponent<DocumentPanelController>().panelShowed = true;

            Sprite spriteDocument = UnityEngine.Resources.Load<Sprite>("Sprites/Camera/Fotografias/" + id);

            GameObject imageDocument = panelFotos.transform.GetChild(2).GetChild(0).gameObject;

            imageDocument.GetComponent<Image>().sprite = spriteDocument;

            await panelFotos.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetUpdate(true)
                .AsyncWaitForCompletion();
            imageDocument.transform.parent.DOKill();
        }

        public async void showObject()
        {
            if (id.Equals("")) return;

            GameObject panelObjects = GameObject.Find("PanelObjects");

            panelObjects.transform.Find("CloseButton").GetComponent<DocumentPanelController>().panelShowed = true;

            Inventory inventory =
                AssetDatabase.LoadAssetAtPath<Inventory>("Assets/Resources/Scripts/ScriptableObjetcts/Objects/InventoryObjects.asset");

            Sprite spriteObj = null;

            foreach (var obj in inventory.InventoryObjects)
            {
                if (obj.key.Equals(id))
                {
                    spriteObj = obj.spriteInventory;
                    break;
                }
            }

            GameObject imageDocument = panelObjects.transform.GetChild(2).gameObject;

            imageDocument.GetComponent<Image>().sprite = spriteObj;
            panelObjects.transform.GetChild(3).gameObject.GetComponent<ChangeTextController>().changeText(id);

            await panelObjects.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetUpdate(true)
                .AsyncWaitForCompletion();
            imageDocument.transform.parent.DOKill();
        }
    }
}