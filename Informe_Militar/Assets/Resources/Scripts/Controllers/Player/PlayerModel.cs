using UnityEngine;

namespace Resources.Scripts.Controllers.Player
{
    public class PlayerModel : MonoBehaviour
    {
        public static PlayerModel instance;
        
        private void Awake()
        {
            instance = this;
        }

        public bool canMove = true;
    }
}