using UnityEngine;

namespace Resources.Scripts.NPCs
{
    public class FrogController : MonoBehaviour
    {
        public void competeMision(string idMision)
        {
            transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        }
    }
}