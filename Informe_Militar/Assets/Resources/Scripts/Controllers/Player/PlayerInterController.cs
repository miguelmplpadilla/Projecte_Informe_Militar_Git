using System.Linq;
using DG.Tweening;
using Resources.Scripts.Controllers.Player;
using Unity.Collections;
using UnityEngine;

public class PlayerInterController : MonoBehaviour
{
    public SphereCollider sphereCollider;

    public InteractBaseController currentInter;

    [ReadOnly, SerializeField] private bool canInteract;

    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, sphereCollider.radius);

        if (colliders.ToList().FindAll(it => it.GetComponent<InteractBaseController>() != null).Count == 0 && currentInter != null)
        {
            EventBus<ExticInteractEvent>.Raise(new ExticInteractEvent { obj = currentInter.gameObject });
            currentInter = null;
        }
        
        float distance = 10000000;
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out InteractBaseController interactBaseController))
            {
                EventBus<ExticInteractEvent>.Raise(new ExticInteractEvent { obj = collider.gameObject });
                
                float currentDistance = Vector3.Distance(transform.position, collider.transform.position);
                if (currentDistance < distance)
                {
                    currentInter = interactBaseController;
                }
            }
        }
        
        if (currentInter != null && PlayerModel.instance.canMove)
        {
            EventBus<EnterInteractEvent>.Raise(new EnterInteractEvent { obj = currentInter.gameObject });
            if (Input.GetKeyDown(KeyCode.F))
            {
                PlayerMovement3D.instance.animator.transform.DORotateQuaternion(
                    Quaternion.LookRotation((currentInter.transform.position - transform.position).normalized), 0.2f);
                
                if (currentInter.typeInteract == InteractBaseController.TypeInteract.PickUpObject)
                    PickUpItem();
                
                EventBus<InteractEvent>.Raise(new InteractEvent { obj = currentInter.gameObject });
            }
        }

        canInteract = currentInter != null;
    }

    private void PickUpItem()
    {
        PlayerModel.instance.canMove = false;
        string finalTrigger = (currentInter as ObjPickUp).typePickUp == ObjPickUp.TypePickUp.GROUND
            ? "pickItem"
            : "pickItemUp";
        PlayerMovement3D.instance.animator.SetTrigger(finalTrigger);
    }
}
