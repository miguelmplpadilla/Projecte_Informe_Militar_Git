using System;
using System.Collections.Generic;
using UnityEngine;

namespace Resources.Scripts.UI.Inventario
{
    [CreateAssetMenu(fileName = "Documents", menuName = "ScriptableObjects/DocumentosSO")]
    public class Documentos : ScriptableObject
    {
        public List<Documento> documentsList;
    }

    [Serializable]
    public class Documento
    {
        public string documentId;

        public Sprite spriteDocument;

        public bool unlocked;
    }
}