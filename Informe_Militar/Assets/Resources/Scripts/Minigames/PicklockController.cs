using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Resources.Scripts.Controllers.Player;
using UnityEngine;
using Random = UnityEngine.Random;

public class PicklockController : MonoBehaviour
{
    public static PicklockController instance;

    public GameObject picklockObject;
    
    public List<GameObject> picksUp = new List<GameObject>();
    public List<GameObject> picksDown = new List<GameObject>();

    public GameObject downPicklocksParent;

    [Range(1, 4)] public int cantPicks = 1;

    private int indexResolvePick = 0;

    public RectTransform resolvePickRT;
    public RectTransform unlockerRT;

    public CanvasGroup canvasPicklock;

    public bool canResolve = false;
    private bool canPicklock = false;

    private DoorController doorController;
    
    public LocalizableString outOfPicklocksMessage;

    private void Start()
    {
        instance = this;
        
        var sequence = DOTween.Sequence();
        sequence.Append(unlockerRT.DOAnchorPosY(330, 1).SetEase(Ease.Linear).SetUpdate(true));
        sequence.Append(unlockerRT.DOAnchorPosY(-330, 1).SetEase(Ease.Linear).SetUpdate(true));
        sequence.SetLoops(-1);
    }

    public void StartMiniGame(DoorController door)
    {
        canPicklock = true;
        indexResolvePick = 0;
        InventoryManager.instance.RemoveItemFromInventory("picklock", 1);
        doorController = door;
        cantPicks = door.cantPicks;
        StartCoroutine(PicklockMinigame());
    }

    private IEnumerator PicklockMinigame()
    {
        yield return null;
        
        picklockObject.SetActive(true);
        SetPicks();
        
        canvasPicklock.alpha = 1;
        canvasPicklock.blocksRaycasts = true;
    }

    private void Update()
    {
        if (!canPicklock) return;
        
        canResolve = false;
        
        RaycastHit2D[] hits = Physics2D.BoxCastAll(resolvePickRT.transform.position, resolvePickRT.sizeDelta, 0, Vector2.zero);
        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.Equals(unlockerRT.gameObject))
            {
                canResolve = true;
                break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && indexResolvePick < cantPicks)
        {
            if (canResolve)
            {
                ResolvePick();
                return;
            }

            if (!InventoryManager.instance.HasItem("picklock"))
            {
                GameNotificationController.instance.ShowNotification(outOfPicklocksMessage);
                ClosePicklockMinigame();
                return;
            }
            
            InventoryManager.instance.RemoveItemFromInventory("picklock", 1);
        }
        
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
        {
            StartCoroutine(ResolveLock());
            return;
        }
        
        SetPositionResolvePick();
    }

    private IEnumerator ResolveLock()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        downPicklocksParent.transform.DORotate(new Vector3(-90, 11.074f, 0), 0.6f).SetUpdate(true);
        yield return new WaitForSecondsRealtime(0.5f);
        
        ClosePicklockMinigame();
        
        doorController.AnimationOpenDoor();
    }

    private void SetPositionResolvePick()
    {
        resolvePickRT.anchoredPosition = new Vector2(resolvePickRT.anchoredPosition.x, Random.Range(303, -303));
    }

    private void ClosePicklockMinigame()
    {
        canvasPicklock.alpha = 0;
        canvasPicklock.blocksRaycasts = false;
        
        PlayerMovement3D.instance.animator.SetTrigger(PlayerMovement3D.instance.isCrouched ? "crouch" : "up");
        PlayerModel.instance.canMove = true;
        
        picklockObject.SetActive(false);
        canPicklock = false;
    }
}
