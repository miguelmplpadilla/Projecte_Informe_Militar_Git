using System.Threading.Tasks;
using DG.Tweening;
using Resources.Scripts.UI.Inventario;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PickUpController : MonoBehaviour
{
    private InventarioController inventarioController;

    private PausaController pausaController;

    public string id = "";

    [Space(10)]

    public UnityEvent onStartFunctions;
    public UnityEvent onInterFunctions;

    private PlayerModel playerModel;

    private void Start()
    {
        pausaController = GameObject.Find("PanelPausa").GetComponent<PausaController>();
        inventarioController = GameObject.Find("PanelInventario").GetComponent<InventarioController>();

        if (onStartFunctions.GetPersistentEventCount() > 0)
        {
            onStartFunctions.Invoke();
            return;
        }
    }

    public void interEnter(PlayerModel model)
    {
        playerModel = model;
    }

    public void inter(PlayerModel model)
    {
        playerModel = model;

        if (onInterFunctions.GetPersistentEventCount() > 0) {
            onInterFunctions.Invoke();
            return;
        }

        Debug.Log("No functions in list");
        playerModel.canInter = true;
        playerModel.mov = true;
    }

    public void interExit(PlayerModel model)
    {
    }

    public async void PickUpObj()
    {
        Inventory inventory =
            AssetDatabase.LoadAssetAtPath<Inventory>("Assets/Resources/Scripts/ScriptableObjetcts/Objects/InventoryObjects.asset");

        for (int i = 0; i < inventory.InventoryObjects.Count; i++)
        {
            if (inventory.InventoryObjects[i].key.Equals(id))
            {
                inventory.InventoryObjects[i].unlock = true;
                break;
            }
        }

        AssetDatabase.Refresh();

        Animator animatorPlayer = GameObject.Find("Player").GetComponent<Animator>();
        animatorPlayer.SetTrigger("agachar");

        Debug.Log(animatorPlayer.GetCurrentAnimatorStateInfo(0).length);

        await Task.Delay((int)(animatorPlayer.GetCurrentAnimatorStateInfo(0).length * 1000));

        playerModel.canInter = true;
        playerModel.mov = true;

        Destroy(gameObject);
    }

    public async void PickUpDocument()
    {
        GameObject imageDocument = GameObject.Find("ImageDocument");

        Inventory documents =
            AssetDatabase.LoadAssetAtPath<Inventory>("Assets/Resources/Scripts/ScriptableObjetcts/Objects/Documents.asset");

        Sprite spriteDocument = null;

        for (int i = 0; i < documents.InventoryObjects.Count; i++)
        {
            if (documents.InventoryObjects[i].key.Equals(id))
            {
                spriteDocument = documents.InventoryObjects[i].spriteInventory;
                documents.InventoryObjects[i].unlock = true;
                break;
            }
        }

        AssetDatabase.Refresh();

        imageDocument.GetComponent<Image>().sprite = spriteDocument;

        Animator animatorPLayer = GameObject.Find("Player").GetComponent<Animator>();
        animatorPLayer.SetTrigger("agachar");
        
        await Task.Delay(550);

        pausaController.pause();

        await imageDocument.transform.parent.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetUpdate(true).AsyncWaitForCompletion();
        imageDocument.transform.parent.DOKill();

        Destroy(gameObject);
    }

    public void DestroyIfUnlockedInventory(string so)
    {
        Inventory inventory =
            AssetDatabase.LoadAssetAtPath<Inventory>("Assets/Resources/Scripts/ScriptableObjetcts/Objects/" + so + ".asset");

        for (int i = 0; i < inventory.InventoryObjects.Count; i++)
        {
            if (inventory.InventoryObjects[i].key.Equals(id) && 
                inventory.InventoryObjects[i].unlock)
                Destroy(gameObject);
        }
    }
}
