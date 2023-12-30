using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DG.Tweening;
using Resources.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogeController : MonoBehaviour
{
    public TextAnimationController textAnimationController;
    private PlayerModel _playerModel;

    public GameObject botonPrefab;
    private GameObject continerBotones1;
    private GameObject continerBotones2;

    private Passage siguietenPassage;

    private bool multiOpcion = false;

    public Dictionary<string, Passage> dialogos = new Dictionary<string, Passage>();
    public Story story;

    public string voiceTone;

    public GameObject npc;

    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    void Start()
    {
        textAnimationController = GameObject.Find("TextoNpc").GetComponent<TextAnimationController>();
        continerBotones1 = GameObject.Find("ContentBotones1");
        continerBotones2 = GameObject.Find("ContentBotones2");
        
        if(!SceneManager.GetActiveScene().name.Equals("TextosPruebaEscena")) 
            _playerModel = GameObject.Find("Player").GetComponent<PlayerModel>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private async void Update()
    {
        if (multiOpcion || npc == null) return;

        await Task.Delay(1);

        if (playerControls.Gameplay.Inter.WasPressedThisFrame())
            iniciarMostrarTexto(siguietenPassage);
    }

    public void startDialoge(Passage p, Dictionary<string, Passage> d, GameObject n, Story s, string voice)
    {
        dialogos = new Dictionary<string, Passage>(d);
        npc = n;
        story = s;
        voiceTone = voice;

        //botones.GetComponent<RectTransform>().anchoredPosition = new Vector2(18.04163f, -86.18922f);
        transform.parent.localScale = Vector3.one;
        
        iniciarMostrarTexto(p);
    }

    public async void iniciarMostrarTexto(Passage text)
    {
        GameObject.Find("NavigationManager").GetComponent<NavigationController>().SetNavigationButtons(null);

        if (text != null)
        {
            setPassageUnique(text);
            string textoMostrar = montarString(text.text);

            textAnimationController.iniciarMostrarTexto(textoMostrar, voiceTone, npc);

            createOptionsArroundPoint(text);

            ejecutarTags(text);
            return;
        }

        if(!SceneManager.GetActiveScene().name.Equals("TextosPruebaEscena")) 
           JSONConverter.rewriteJson(story);

        transform.parent.localScale = Vector3.zero;

        Debug.Log("Dialoge: "+ _playerModel.canInter + " Frame: " + Time.frameCount);

        npc = null;

        if (_playerModel == null) return;
        _playerModel.canInter = true;
        _playerModel.mov = true;
    }

    private void setPassageUnique(Passage passage)
    {
        for (int i = 0; i < story.passages.Count; i++)
        {
            if (!passage.name.Equals(story.passages[i].name)) continue;

            foreach (var tg in story.passages[i].tags)
            {
                string[] splitTags = Regex.Split(tg, ">");
                if (splitTags[1].Equals("unique"))
                {
                    story.passages[i].ussed = true;
                    return;
                }
            }
        }
    }

    public string montarString(string stringMontar)
    {
        string stringReturn = "";

        for (int i = 0; i < stringMontar.Length; i++)
        {
            if (stringMontar[i].Equals('[')) break;
            
            stringReturn += stringMontar[i];
        }

        return stringReturn;
    }

    public void createOptionsArroundPoint(Passage passage)
    {
        GameObject[] botonesEnEscena = GameObject.FindGameObjectsWithTag("BotonOpcion");

        for (int i = 0; i < botonesEnEscena.Length; i++)
        {
            Destroy(botonesEnEscena[i]);
        }

        multiOpcion = false;
        siguietenPassage = null;

        List<GameObject> listBotones = new List<GameObject>();

        if (passage.links == null) return;
        
        if (passage.links.Count > 1 || getShowButton(passage))
        {
            multiOpcion = true;
            int index = 1;

            foreach (var link in passage.links)
            {
                if (dialogos[link.name].ussed) continue;
                
                int cantButtonCompare = passage.links.Count > 2 ? 2 : 1;
                Transform continer = index <= cantButtonCompare
                    ? continerBotones2.transform
                    : continerBotones1.transform;
                if (getAllButtonsToShow(passage) == 1) 
                    continer = continerBotones1.transform;
                
                GameObject boton = Instantiate(botonPrefab, continer);
                listBotones.Add(boton);

                boton.GetComponentInChildren<TextMeshProUGUI>().text =
                    dialogos[link.name].name;

                boton.GetComponent<BotonOpcionController>().passage = dialogos[link.name];

                index++;
            }

            NavigationButtons navigateButtons = new NavigationButtons();
            navigateButtons.direction = NavigationButtons.DirectionType.All;
            Buttons button = new Buttons();

            if (continerBotones1.transform.childCount > 0)
            {
                for (int i = 0; i < continerBotones1.transform.childCount; i++)
                {
                    button.horizontalButtons.Add(continerBotones1.transform.GetChild(i).gameObject);
                }
                navigateButtons.verticalButtons.Add(button);
            }

            if (continerBotones2.transform.childCount > 0)
            {
                button = new Buttons();
                for (int i = 0; i < continerBotones2.transform.childCount; i++)
                {
                    button.horizontalButtons.Add(continerBotones2.transform.GetChild(i).gameObject);
                }
                navigateButtons.verticalButtons.Add(button);
            }

            GameObject.Find("NavigationManager").GetComponent<NavigationController>().SetNavigationButtons(navigateButtons);

            if (listBotones.Count == 0)
            {
                siguietenPassage = null;
                multiOpcion = false;
            }

            return;
        }
        
        siguietenPassage = passage.links.Count > 0 ? dialogos[passage.links[0].name] : null;
    }

    private Passage getPassageFromLink(string namePassage)
    {
        foreach (var passage in story.passages)
        {
            if (passage.name.Equals(namePassage)) return passage;
        }

        return null;
    }

    private bool getShowButton(Passage passage)
    {
        foreach (var link in passage.links)
            return getShowLinkButton(getPassageFromLink(link.name));

        return false;
    }

    private bool getShowLinkButton(Passage passage)
    {
        if (passage.tags == null) return false;

        for (int i = 0; i < passage.tags.Count; i++)
        {
            string[] splitTags = Regex.Split(passage.tags[i], ">");
            if (splitTags[1].Equals("showButton"))
                return true;
        }

        return false;
    }

    private int getAllButtonsToShow(Passage passage)
    {
        int num = 0;
        foreach (var link in passage.links)
        {
            Passage passageLink = getPassageFromLink(link.name);
            if ((getShowLinkButton(passageLink) && !passageLink.ussed) || !passageLink.ussed) num++;
        }

        return num;
    }

    public void ejecutarTags(Passage pasage)
    {
        if (pasage.tags == null) return;

        for (int i = 0; i < pasage.tags.Count; i++)
        {
            string[] splitTags = Regex.Split(pasage.tags[i], ">");
            if (splitTags[0].Equals("Function"))
            {
                writeTextFunction(splitTags[1]);
                npc.BroadcastMessage(splitTags[1]);
            }
        }
    }

    private void writeTextFunction(string function)
    {
        string scene = SceneManager.GetActiveScene().name;

        if (!scene.Equals("TextosPruebaEscena")) return;
        
        TextMeshProUGUI textFunction = GameObject.Find("TextoEjecucionFuncion").GetComponent<TextMeshProUGUI>();
        
        Color colorText = textFunction.color;
        colorText.a = 1;
        textFunction.color = colorText;

        textFunction.text = "Se ha ejecutado funcion con nombre: " + function;
        
        colorText.a = 0;

        textFunction.DOColor(colorText, 5);
    }
}