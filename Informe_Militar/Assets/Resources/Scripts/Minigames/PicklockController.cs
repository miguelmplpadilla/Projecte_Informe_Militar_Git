using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class PicklockController : MonoBehaviour
{
    public List<GameObject> picksUp = new List<GameObject>();
    public List<GameObject> picksDown = new List<GameObject>();

    public GameObject downPicklocksParent;

    [Range(1, 4)] public int cantPicks = 1;

    private int indexResolvePick = 0;

    public CanvasGroup canvasPicklock;

    public void StartMiniGame()
    {
        canvasPicklock.alpha = 1;
        canvasPicklock.blocksRaycasts = true;
        
        InventoryManager.instance.RemoveItemFromInventory("picklock", 1);
        
        SetPicks();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) SetPicks();
        if (Input.GetKeyDown(KeyCode.T) && indexResolvePick < cantPicks) ResolvePick();
    }

    private void SetPicks()
    {
        foreach (var pick in picksUp)
            pick.transform.localScale = Vector3.zero;
        
        foreach (var pick in picksDown)
            pick.transform.localScale = Vector3.zero;
        
        for (int i = 0; i < cantPicks; i++)
        {
            picksUp[i].transform.localScale = Vector3.one;
            picksDown[i].transform.localScale = Vector3.one;
            
            float firstScale = Random.Range(0.3f, 0.8f);
            float secondScale = 1 - firstScale;
            firstScale -= 0.02f;
            secondScale -= 0.02f;

            picksUp[i].transform.GetChild(0).localScale = new Vector3(picksUp[i].transform.GetChild(0).localScale.x,
                firstScale, picksUp[i].transform.GetChild(0).localScale.z);
            picksDown[i].transform.GetChild(0).localScale = new Vector3(picksDown[i].transform.GetChild(0).localScale.x,
                secondScale, picksDown[i].transform.GetChild(0).localScale.z);
        }
    }

    private void ResolvePick()
    {
        picksUp[indexResolvePick].transform.GetChild(0).GetChild(0).DOLocalMoveY(0, 0.6f);
        indexResolvePick++;
        if (indexResolvePick >= cantPicks)
            StartCoroutine(ResolveLock());
    }

    private IEnumerator ResolveLock()
    {
        yield return new WaitForSeconds(1.5f);
        downPicklocksParent.transform.DORotate(new Vector3(-90, 0, 0), 0.6f);
    }
}
