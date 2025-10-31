using UnityEngine;

namespace Resources.Scripts.Controllers.Player
{
    public class PlayerAnimatorManager : MonoBehaviour
    {
        public void SetCanMoveTrue()
        {
            PlayerModel.instance.canMove = true;
        }
    }
}