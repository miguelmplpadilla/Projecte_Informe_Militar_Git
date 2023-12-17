using Resources.Scripts;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InterController : MonoBehaviour, InterBaseInterface
{
    private GameObject buttonInter;

    public bool initialInter = false;
    public bool canInter = true;

    private void Start()
    {
        buttonInter = transform.Find("ButtonInter").gameObject;
    }

    public void interEnter(PlayerModel model)
    {
        if (initialInter || !canInter) return;
        
        transform.parent.gameObject.SendMessage("interEnter", model);
        buttonInter.SendMessage("mostrar", "");
        initialInter = true;
    }

    public void inter(PlayerModel model)
    {
        if (!canInter)
        {
            model.canInter = true;
            model.mov = true;
            return;
        }

        transform.parent.gameObject.SendMessage("inter", model);
    }

    public void interExit(PlayerModel model)
    {
        if (!initialInter || !canInter) return;

        transform.parent.gameObject.SendMessage("interExit", model);
        buttonInter.SendMessage("esconder");
        initialInter = false;
    }
}
