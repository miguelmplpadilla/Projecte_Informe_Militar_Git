using Resources.Scripts;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InterController : MonoBehaviour, InterBaseInterface
{
    private GameObject buttonInter;

    public bool initialInter = false;

    private void Start()
    {
        buttonInter = transform.Find("ButtonInter").gameObject;
    }

    public void interEnter(PlayerModel model)
    {
        if (initialInter) return;
        
        transform.parent.gameObject.SendMessage("interEnter", model);
        buttonInter.SendMessage("mostrar", "");
        initialInter = true;
    }

    public void inter(PlayerModel model)
    {
        transform.parent.gameObject.SendMessage("inter", model);
        Debug.Log("Inter: " + transform.parent.gameObject.name);
    }

    public void interExit(PlayerModel model)
    {
        if (!initialInter) return;
        
        transform.parent.gameObject.SendMessage("interExit", model);
        buttonInter.SendMessage("esconder");
        initialInter = false;
    }
}
