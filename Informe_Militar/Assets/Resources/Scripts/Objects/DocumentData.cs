using System;
using Resources.Scripts.Inventory;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Resources.Scripts.Objects
{
    [Serializable]
    public class DocumentData : ItemData
    {
        public string keyDocument;
        
        [SerializeField] private Sprite frontES;
        [SerializeField] private Sprite frontEN;
        public LocalizableString textFront;
        public LocalizableString descriptionFront;
    
        [SerializeField] private Sprite backES;
        [SerializeField] private Sprite backEN;
        public LocalizableString textBack;
        public LocalizableString descriptionBack;

        public bool isUnlocked = false;
        
        public Sprite ValueFront
        {
            get
            {
                var locale = LocalizationSettings.SelectedLocale;

                switch (locale.Identifier.Code)
                {
                    case "es-ES":
                        return frontES;
                    case "en-US":
                        return frontEN;
                    default:
                        return frontEN;
                }
            }
        }
        
        public Sprite ValueBack
        {
            get
            {
                var locale = LocalizationSettings.SelectedLocale;

                switch (locale.Identifier.Code)
                {
                    case "es-ES":
                        return backES;
                    case "en-US":
                        return backEN;
                    default:
                        return backEN;
                }
            }
        }

        public DocumentData(ItemData itemData, DocumentData documentData) : base(itemData)
        {
            keyDocument = documentData.keyDocument;
            frontES = documentData.frontES;
            frontEN = documentData.frontEN;
            textFront = documentData.textFront;
            descriptionFront = documentData.descriptionFront;
            backES = documentData.backES;
            backEN = documentData.backEN;
            textBack = documentData.textBack;
            descriptionBack = documentData.descriptionBack;
        }
    }
}