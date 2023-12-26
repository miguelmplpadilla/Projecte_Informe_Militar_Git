using DG.Tweening;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;

namespace Resources.Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        private Image image;
        public KeyCode key;

        private void Start()
        {
            image = GetComponent<Image>();
            SetSprite();
        }

        private void LateUpdate()
        {
            SetSprite();
        }

        public void SetSprite()
        {
            image.sprite = ButtonUIManager.GetSprite(key);
        }

        public void SetKey(KeyCode keyCode) 
        {
            key = keyCode;
        }
    }
}