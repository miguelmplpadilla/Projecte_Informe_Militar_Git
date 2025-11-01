using System;
using TMPro;
using UnityEngine;

namespace Resources.Scripts.Tools
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizableController : MonoBehaviour
    {
        private TextMeshProUGUI text;
        private LocalizableString localizableString;

        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            if (localizableString != null) 
                text.text = localizableString.Value;
        }

        public void SetText(LocalizableString localizable)
        {
            localizableString = localizable;
        }
    }
}