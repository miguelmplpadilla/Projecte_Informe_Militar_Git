using Resources.Scripts;
using Resources.Scripts.Dialogos;
using UnityEngine;

public class NPCController : MonoBehaviour, InterBaseInterface
{
    private GameObject _player;

    public string json;

    public string pathJson1 = "Dialogo1";
    public string pathJson2 = "Dialogo2";
    public string pathJsonEndMission = "Dialogo3";

    public string nombreMetodoDialogo;
    
    private DialogeController _dialogeController;

    private Animator _animator;

    public GameObject missionPopup;

    private bool misionCompleted = false;

    void Start()
    {
        if (PlayerPrefs.HasKey(name))
            json = PlayerPrefs.GetString(name);
        else
            json = pathJson1;

        if (pathJsonEndMission.Equals(json))
        {
            misionCompleted = true;
            SendMessage("competeMision", "default");
        }

        _animator = GetComponentInChildren<Animator>();
        
        _player = GameObject.Find("Player");
        _dialogeController = GameObject.Find("PanelDialogo").GetComponent<DialogeController>();
    }
    
    public void interEnter(PlayerModel model)
    {
    }

    public void inter(PlayerModel model)
    {
        if (!nombreMetodoDialogo.Equals("") && json.Equals(pathJson2))
            ejecutarMetodoDialogo(nombreMetodoDialogo);
        
        //RootStory root = JSONConverter.parseJson(json);
        //_dialogeController.StartDialoge(gameObject, root);
        
        if (json.Equals(pathJson1))
            json = pathJson2;

        PlayerPrefs.SetString(name, json);
    }

    public void interExit(PlayerModel model)
    {
    }

    public void ejecutarMetodoDialogo(string nombreMetodo)
    {
        string[] metodos = nombreMetodo.Split("=");
        SendMessage(metodos[0], metodos[1]);
    }

    public void changeAnimation(string trigger)
    {
        _animator.SetTrigger(trigger);
    }
}
