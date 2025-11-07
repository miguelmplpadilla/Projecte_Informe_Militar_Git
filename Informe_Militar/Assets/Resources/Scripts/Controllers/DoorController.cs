using System.Collections;
using DG.Tweening;
using UnityEngine;

public class DoorController : InteractBaseController
{
    public bool isLocked = false;
    public bool isOpened = false;

    protected override IEnumerator Inter()
    {
        if (isOpened) yield break;
        if (isLocked)
        {
            if (InventoryManager.instance.HasItem("picklock"))
            {
                //TODO: Iniciar minijuego de ganzua
                Debug.Log("Minijuego ganzua");
            }
            else
            {
                //TODO: Hacer aviso general para mostar mensajes
                Debug.Log("El jugador no tiene ninguna ganzua");
            }
            
            yield break;
        }
        
        AnimationOpenDoor();
    }

    public void AnimationOpenDoor()
    {
        isOpened = true;
        transform.DORotate(new Vector3(0, -90, 0), 0.8f);
    }
}
