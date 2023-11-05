using DG.Tweening;
using UnityEngine;

public class DocumentPanelController : MonoBehaviour
{
    private PausaController pausaController;

    private void Start()
    {
        pausaController = GameObject.Find("PanelPausa").GetComponent<PausaController>();
    }
    
    public void closePanel()
    {
        gameObject.transform.parent.DOScale(Vector3.zero, 0.5f).SetUpdate(true).SetEase(Ease.InBack);
        
        if (GameObject.Find("PanelInventario").transform.localScale.x > 0) return;
        
        pausaController.pause();
        
        PlayerModel model = GameObject.Find("Player").GetComponent<PlayerModel>();
        model.mov = true;
        model.canInter = true;
    }
}
