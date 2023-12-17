using Resources.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class AllySupplyController : MonoBehaviour, InterBaseInterface
{
    public int cantToPress;
    public int cantPressed;

    public Image imageBarSupplyRight;
    public Image imageBarSupplyLeft;

    private RunningController runningController;

    public bool helped = false;

    private void Start()
    {
        runningController = GameObject.Find("RunningController").GetComponent<RunningController>();
        cantToPress = Random.Range(5, 15);
    }

    public void inter(PlayerModel model)
    {
        model.canInter = true;
        model.mov = true;

        cantPressed++;

        imageBarSupplyRight.fillAmount = (float) cantPressed / cantToPress;
        imageBarSupplyLeft.fillAmount = (float) cantPressed / cantToPress;

        if (cantPressed >= cantToPress) DoneSupply();
    }

    public void interEnter(PlayerModel model)
    {
    }

    public void interExit(PlayerModel model)
    {
    }

    private void DoneSupply()
    {
        helped = true;
        runningController.SetCanCreateAlly(true);
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        runningController.SetCanCreateAlly(true, false);

        if (helped)
        {
            runningController.RestAllyHelped();
        }
    }
}
