using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using Resources.Scripts.UI.Inventario;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventarioController : MonoBehaviour
{
    private List<GameObject> createdButtons = new List<GameObject>();

    public GameObject prefabButtonDocumento;
    public GameObject prefabButtonFoto;
    public GameObject prefabButtonObject;

    public GameObject contentDocumentos;
    public GameObject contentFotos;
    public GameObject contentObjetos;

    private GameObject actualPanelShowed;
    private bool showingPanel = false;

    private bool showingInventario = false;
    private bool inventarioShowed = false;

    private PlayerModel model;
    private PausaController pausaController;

    private NavigationController navigationController;
    public NavigationButtons optionsNavigationButtons;
    public int indexOptions = 0;

    private UIInput uiInput;

    private void Awake()
    {
        uiInput = new UIInput();

        uiInput.Navigation.NavigateInventory.performed += MoveOptions;
    }

    private void Start()
    {
        navigationController = GameObject.Find("NavigationManager").GetComponent<NavigationController>();
        pausaController = GameObject.Find("PanelPausa").GetComponent<PausaController>();
        model = GameObject.Find("Player").GetComponent<PlayerModel>();
        actualPanelShowed = GameObject.Find("PanelDocumentos");
    }

    private void OnEnable()
    {
        uiInput.Enable();
    }

    private void OnDisable()
    {
        uiInput.Disable();
    }

    private void Update()
    {
        if (uiInput.UISelf.OpenInventory.WasPressedThisFrame() && !showingInventario)
        {
            pausaController.pause();
            
            if (!inventarioShowed) 
                showInventario();
            else
                hideInventario();
            
            inventarioShowed = !inventarioShowed;
            showingInventario = true;
        }
    }

    private void MoveOptions(InputAction.CallbackContext context)
    {
        if (!inventarioShowed || Input.GetJoystickNames().Length == 0 || showingInventario || showingPanel) return;

        int lastIndexOptions = indexOptions;

        indexOptions += context.ReadValue<float>() > 0 ? 1 : -1;

        if (indexOptions >= optionsNavigationButtons.verticalButtons.Count)
            indexOptions = 0;
        else if (indexOptions < 0)
            indexOptions = optionsNavigationButtons.verticalButtons.Count - 1;

        optionsNavigationButtons.verticalButtons[0].horizontalButtons[indexOptions].GetComponent<Button>().onClick.Invoke();
        optionsNavigationButtons.verticalButtons[0].horizontalButtons[indexOptions].GetComponent<Image>().color = Color.red;
        optionsNavigationButtons.verticalButtons[0].horizontalButtons[lastIndexOptions].GetComponent<Image>().color = Color.white;
    }

    public async void showInventario()
    {
        createDocumentButtons();
        createFotoButtons();
        CreateObjectsButtons();

        optionsNavigationButtons.verticalButtons[0].horizontalButtons[0].GetComponent<Button>().onClick.Invoke();
        optionsNavigationButtons.verticalButtons[0].horizontalButtons[0].GetComponent<Image>().color = Color.red;
        indexOptions = 0;

        await transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetUpdate(true).AsyncWaitForCompletion();

        showingInventario = false;
    }

    public async void hideInventario()
    {
        await transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).SetUpdate(true).AsyncWaitForCompletion();
        
        destroyButtons();
        
        showingInventario = false;
    }

    private void destroyButtons()
    {
        foreach (var button in createdButtons)
        {
            Destroy(button);
        }
        
        createdButtons.Clear();
    }
    
    public async void changePanel(string panelName)
    {
        if (panelName.Equals(actualPanelShowed.name) || showingPanel) return;

        showingPanel = true;

        GameObject panel = GameObject.Find(panelName);
        panel.transform.SetAsLastSibling();

        panel.transform.localScale = Vector3.one;

        Vector3 originalPanelPosition = panel.transform.localPosition;

        await panel.transform.DOLocalMoveY(actualPanelShowed.transform.localPosition.y, 0.5f).SetUpdate(true)
            .SetEase(Ease.OutBack).AsyncWaitForCompletion();
        
        actualPanelShowed.transform.localScale = Vector3.zero;
        actualPanelShowed.transform.localPosition = originalPanelPosition;
        actualPanelShowed = panel;

        showingPanel = false;
    }

    public void StartNavigationButtons(GameObject continer)
    {
        NavigationButtons navigationButtons = new NavigationButtons();


    }

    private void createDocumentButtons()
    {
        Inventory documentos =
            AssetDatabase.LoadAssetAtPath<Inventory>("Assets/Resources/Scripts/ScriptableObjetcts/Objects/Documents.asset");

        foreach (var documento in documentos.InventoryObjects)
        {
            if (documento.unlock)
            {
                GameObject button = Instantiate(prefabButtonDocumento, contentDocumentos.transform);
                button.transform.GetChild(1).GetComponent<ChangeTextController>().changeText(documento.key);
                button.GetComponent<ButtonViewController>().id = documento.key;
                createdButtons.Add(button);
            }
        }
    }

    private void createFotoButtons()
    {
        foreach (var foto in UnityEngine.Resources.LoadAll<Sprite>("Sprites/Camera/Fotografias"))
        {
            GameObject button = Instantiate(prefabButtonFoto, contentFotos.transform);
            button.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = foto.name;
            button.GetComponent<ButtonViewController>().id = foto.name;
            createdButtons.Add(button);
        }
    }

    private void CreateObjectsButtons()
    {
        Inventory inventory =
            AssetDatabase.LoadAssetAtPath<Inventory>("Assets/Resources/Scripts/ScriptableObjetcts/Objects/InventoryObjects.asset");

        foreach (var obj in inventory.InventoryObjects)
        {
            GameObject button = Instantiate(prefabButtonObject, contentObjetos.transform);
            Image imageObj = button.transform.GetChild(0).GetComponent<Image>();
            imageObj.sprite = obj.spriteInventory;
            createdButtons.Add(button);
            if (!obj.unlock)
            {
                imageObj.color = Color.black;
                return;
            }
            button.GetComponent<ButtonViewController>().id = obj.key;
        }
    }

    private void rewriteJSON(string jsonName, Documentos objType)
    {
        string idioma = IdiomaController.getLanguage();
        
        string json = JsonUtility.ToJson(objType);
        
        File.WriteAllText(Application.dataPath+"/Resources/JSON/"+idioma+"/JSONInventario/"+jsonName+".json", json);
    }
}
