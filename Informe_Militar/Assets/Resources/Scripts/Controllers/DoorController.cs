using System.Collections;
using DG.Tweening;
using UnityEngine;

public class DoorController : InteractBaseController
{
    public bool isLocked = false;
    
    [Range(1, 4)] public int cantPicks = 1;
    
    public LocalizableString doorLockedMessage;

    protected override IEnumerator Inter()
    {
        if (isLocked)
        {
            if (InventoryManager.instance.HasItem("picklock"))
            {
                PicklockController.instance.StartMiniGame(this);
                PlayerMovement3D.instance.animator.SetTrigger("picklock");
            }
            else
            {
                GameNotificationController.instance.ShowNotification(doorLockedMessage);
                PlayerMovement3D.instance.animator.SetTrigger("openDoor");
                Debug.Log("El jugador no tiene ninguna ganzua");
            }
            
            yield break;
        }
        
        PlayerMovement3D.instance.animator.SetTrigger("openDoor");

        yield return new WaitForSeconds(1);
        
        AnimationOpenDoor();
    }

    public void AnimationOpenDoor()
    {
        canInteract = false;
        transform.DORotate(new Vector3(0, -90, 0), 0.8f);
    }
}
