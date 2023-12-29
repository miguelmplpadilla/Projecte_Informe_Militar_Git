using DG.Tweening;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Windows;

public class DocumentPanelController : MonoBehaviour
{
    private PausaController pausaController;

    private UIInput uiInput;

    public bool panelShowed = false;

    private void Awake()
    {
        uiInput = new UIInput();
    }

    private void Start()
    {
        pausaController = GameObject.Find("PanelPausa").GetComponent<PausaController>();
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
        if (uiInput.Navigation.Close.WasPressedThisFrame()) closePanel();
    }

    public void closePanel()
    {
        if (!panelShowed) return;

        panelShowed = false;

        gameObject.transform.parent.DOScale(Vector3.zero, 0.5f).SetUpdate(true).SetEase(Ease.InBack);
        
        if (GameObject.Find("PanelInventario").transform.localScale.x > 0) return;
        
        pausaController.pause();
        
        PlayerModel model = GameObject.Find("Player").GetComponent<PlayerModel>();
        model.mov = true;
        model.canInter = true;
    }
}
