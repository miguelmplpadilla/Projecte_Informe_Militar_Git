using System;
using System.Collections.Generic;

namespace Resources.Scripts.UI.Inventario
{
    [Serializable]
    public class Documentos
    {
        public List<Documento> documentsList;
    }

    [Serializable]
    public class Documento
    {
        public string documentId;
        public string documentName;
        public bool unlocked;
    }
}