using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using Resources.Scripts.UI.Inventario;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventarioController : MonoBehaviour
{
    private List<GameObject> createdButtons = new List<GameObject>();
    public List<GameObject> currentContinerButtons = new List<GameObject>();

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

    public int indexContiner = 0;

    private void Awake()
    {
        uiInput = new UIInput();

        uiInput.Navigation.NavigateInventory.performed += MoveOptions;
        uiInput.Navigation.NavigateButtons.performed += MoveContinerButtons;
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
        if (uiInput.UISelf.OpenInventory.WasPressedThisFrame() && !showingInventario && !model.isPaused)
        {
            pausaController.pause();
            model.canPause = !model.canPause;

            if (!inventarioShowed) 
                showInventario();
            else
                hideInventario();
            
            inventarioShowed = !inventarioShowed;
            showingInventario = true;
        }
    }

    private void MoveContinerButtons(InputAction.CallbackContext context)
    {
        GameObject continer = actualPanelShowed.transform.GetChild(0).gameObject;
        Debug.Log(continer.name);
        ShowButtons(context.ReadValue<float>() > 0 ? 1 : -1);
        Navigate(continer);
    }

    public void ShowButtons(int sumIndex)
    {
        if (currentContinerButtons.Count == 0) return;

        indexContiner += sumIndex;

        if (indexContiner > currentContinerButtons.Count / 10)
            indexContiner = currentContinerButtons.Count / 10;
        else if (indexContiner < 0)
            indexContiner = 0;

        for (int i = 0; i < currentContinerButtons.Count; i++)
        {
            currentContinerButtons[i].SetActive(false);
        }

        for (int i = 10 * indexContiner; i < (indexContiner * 10) + 10; i++)
        {
            if (i >= currentContinerButtons.Count) break;
            currentContinerButtons[i].SetActive(true);
        }
    }

    private void MoveOptions(InputAction.CallbackContext context)
    {
        if (!inventarioShowed || Input.GetJoystickNames().Length == 0 || showingInventario || showingPanel) return;

        int lastIndexOptions = indexOptions;

        indexOptions += context.ReadValue<float>() > 0 ? 1 : -1;

        if (indexOptions >= optionsNavigationButtons.verticalButtons[0].horizontalButtons.Count)
            indexOptions = 0;
        else if (indexOptions < 0)
            indexOptions = optionsNavigationButtons.verticalButtons[0].horizontalButtons.Count - 1;

        optionsNavigationButtons.verticalButtons[0].horizontalButtons[indexOptions].GetComponent<Button>().onClick.Invoke();
        optionsNavigationButtons.verticalButtons[0].horizontalButtons[indexOptions].GetComponent<Image>().color = Color.red;
        optionsNavigationButtons.verticalButtons[0].horizontalButtons[lastIndexOptions].GetComponent<Image>().color = Color.white;
    }

    public void SetIndexOptionsButton(int indexButton)
    {
        int lastIndexOptions = indexOptions;

        indexOptions = indexButton;

        optionsNavigationButtons.verticalButtons[0].horizontalButtons[indexOptions].GetComponent<Image>().color = Color.red;
        optionsNavigationButtons.verticalButtons[0].horizontalButtons[lastIndexOptions].GetComponent<Image>().color = Color.white;
    }

    public void Navigate(GameObject continer)
    {
        NavigationButtons navigationButtons = new NavigationButtons();

        navigationButtons.direction = NavigationButtons.DirectionType.All;

        int countChild = 0;

        List<GameObject> activeButtonContiner = new List<GameObject>();
        for (int i = 0; i < continer.transform.childCount; i++)
        {
            GameObject button = continer.transform.GetChild(i).gameObject;
            if (button.activeSelf) activeButtonContiner.Add(button);
        }

        if (continer.transform.childCount < 2)
        {
            Buttons buttons = new Buttons();
            buttons.horizontalButtons.Add(continer.transform.GetChild(0).gameObject);
            navigationButtons.verticalButtons.Add(buttons);
            navigationController.SetNavigationButtons(navigationButtons);
            return;
        }

        for (int i = 0; i < (activeButtonContiner.Count / continer.GetComponent<GridLayoutGroup>().constraintCount)+1; i++)
        {
            Buttons buttons = new Buttons();

            for (int j = 0; j < continer.GetComponent<GridLayoutGroup>().constraintCount; j++)
            {
                if (countChild >= activeButtonContiner.Count)
                {
                    if (buttons.horizontalButtons.Count > 0) 
                        navigationButtons.verticalButtons.Add(buttons);
                    navigationController.SetNavigationButtons(navigationButtons);
                    return;
                }

                buttons.horizontalButtons.Add(activeButtonContiner[countChild].gameObject);

                countChild++;
            }

            if (buttons.horizontalButtons.Count == 0) continue;

            navigationButtons.verticalButtons.Add(buttons);
        }

        navigationController.SetNavigationButtons(navigationButtons);
    }

    public async void showInventario()
    {
        createDocumentButtons();
        createFotoButtons();
        CreateObjectsButtons();

        StartNavigationButtons(contentDocumentos);

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

    private void SetCurrentButtonsContiner(GameObject continer)
    {
        if (continer.transform.childCount == 0) return;

        currentContinerButtons = new List<GameObject>();

        for (int i = 0; i < continer.transform.childCount; i++)
        {
            currentContinerButtons.Add(continer.transform.GetChild(i).gameObject);
        }
    }

    public void StartNavigationButtons(GameObject continer)
    {
        if (continer.transform.childCount == 0) return;

        SetCurrentButtonsContiner(continer);

        ShowButtons(0);

        Navigate(continer);
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
            button.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = foto;
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
