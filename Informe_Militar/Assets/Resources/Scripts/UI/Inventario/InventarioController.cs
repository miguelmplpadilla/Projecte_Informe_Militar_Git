using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using Resources.Scripts.UI.Inventario;
using TMPro;
using UnityEngine;

public class InventarioController : MonoBehaviour
{
    public Documentos documentos = new Documentos();

    private List<GameObject> createdButtons = new List<GameObject>();

    public GameObject prefabButtonDocumento;
    public GameObject prefabButtonFoto;

    public GameObject contentDocumentos;
    public GameObject contentFotos;

    private GameObject actualPanelShowed;
    private bool showingPanel = false;

    private bool showingInventario = false;
    private bool inventarioShowed = false;

    private PlayerModel model;
    private PausaController pausaController;

    private void Start()
    {
        pausaController = GameObject.Find("PanelPausa").GetComponent<PausaController>();
        model = GameObject.Find("Player").GetComponent<PlayerModel>();
        actualPanelShowed = GameObject.Find("PanelDocumentos");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !showingInventario)
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

    public async void showInventario()
    {
        initializeObjs();
        createDocumentButtons();
        createFotoButtons();
        
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

    private void initializeObjs()
    {
        string idioma = IdiomaController.getLanguage();

        string jsonDocumentos = File.ReadAllText(Application.dataPath + "/Resources/JSON/"+idioma+"/JSONInventario/Documentos.json");

        //TextAsset jsonDocumentos = UnityEngine.Resources.Load<TextAsset>("JSON/"+idioma+"/JSONInventario/Documentos");
        JsonUtility.FromJsonOverwrite(jsonDocumentos, documentos);
    }

    private void createDocumentButtons()
    {
        foreach (var documento in documentos.documentsList)
        {
            if (documento.unlocked)
            {
                GameObject button = Instantiate(prefabButtonDocumento, contentDocumentos.transform);
                button.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = documento.documentName;
                button.GetComponent<ButtonViewController>().id = documento.documentId;
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

    private void rewriteJSON(string jsonName, Documentos objType)
    {
        string idioma = IdiomaController.getLanguage();
        
        string json = JsonUtility.ToJson(objType);
        
        File.WriteAllText(Application.dataPath+"/Resources/JSON/"+idioma+"/JSONInventario/"+jsonName+".json", json);
    }

    public void unlockDocument(string docID)
    {
        for (int i = 0; i < documentos.documentsList.Count; i++)
        {
            if (documentos.documentsList[i].documentId.Equals(docID))
            {
                documentos.documentsList[i].unlocked = true;
                break;
            }
        }
        
        rewriteJSON("Documentos", documentos);
    }
}
