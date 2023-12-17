using DG.Tweening;
using Resources.Scripts;
using UnityEngine;

public class AllyInjured : MonoBehaviour, InterBaseInterface
{
    public GameObject rotator;
    public GameObject zoneToPress;

    public int cantToPress;
    public int cantPressed;

    private RunningController runningController;

    public bool helped = false;

    private void Start()
    {
        runningController = GameObject.Find("RunningController").GetComponent<RunningController>();

        cantToPress = Random.Range(1, 5 + 1);

        zoneToPress.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360 + 1));

        rotator.transform.DORotate(new Vector3(0, 0, 360), 1.5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1).OnComplete(() =>
        {
            rotator.transform.localRotation = Quaternion.Euler(0,0,0);
        });
    }

    public void inter(PlayerModel model)
    {
        if (rotator.transform.eulerAngles.z + 50 > zoneToPress.transform.eulerAngles.z &&
            rotator.transform.eulerAngles.z - 50 < zoneToPress.transform.eulerAngles.z)
            cantPressed++;

        zoneToPress.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 365 + 1));

        model.canInter = true;
        model.mov = true;

        if (cantPressed >= cantToPress)
            DoneSupply();
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
