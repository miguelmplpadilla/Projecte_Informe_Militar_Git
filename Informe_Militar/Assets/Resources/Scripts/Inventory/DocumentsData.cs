using System.Collections.Generic;
using Resources.Scripts.Objects;
using UnityEngine;

namespace Resources.Scripts.Inventory
{
    [CreateAssetMenu(fileName = "DocumentsData", menuName = "ScriptableObjects/DocumentsData")]
    public class DocumentsData : ScriptableObject
    {
        public List<DocumentData> documents = new List<DocumentData>();
    }
}