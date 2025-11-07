using System;
using Resources.Scripts.Inventory;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Resources.Scripts.Objects
{
    [Serializable]
    public class DocumentData : ItemData
    {
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
                if (!isUnlocked) return null;
                
                var locale = LocalizationSettings.SelectedLocale;

                Sprite spriteFinal = null;

                switch (locale.Identifier.Code)
                {
                    case "es-ES":
                        spriteFinal = frontES;
                        break;
                    case "en-US":
                        spriteFinal = frontEN;
                        break;
                    default:
                        spriteFinal = frontEN;
                        break;
                }

                if (spriteFinal == null) spriteFinal = frontEN;
                
                return spriteFinal;
            }
        }
        
        public Sprite ValueBack
        {
            get
            {
                var locale = LocalizationSettings.SelectedLocale;

                Sprite spriteFinal = null;

                switch (locale.Identifier.Code)
                {
                    case "es-ES":
                        spriteFinal = backES;
                        break;
                    case "en-US":
                        spriteFinal = backEN;
                        break;
                    default:
                        spriteFinal = backEN;
                        break;
                }

                if (spriteFinal == null) spriteFinal = backEN;
                
                return spriteFinal;
            }
        }

        public DocumentData(ItemData itemData, DocumentData documentData) : base(itemData)
        {
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