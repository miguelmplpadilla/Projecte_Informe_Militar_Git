using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class PausaController : MonoBehaviour
{
    private PlayerModel _model;

    private bool showingPanel = false;

    private UIInput uiInput;

    public NavigationButtons pauseNavigationButtons;

    private NavigationController navigationController;

    private void Awake()
    {
        uiInput = new UIInput();
    }

    private void Start()
    {
        navigationController = GameObject.Find("NavigationManager").GetComponent<NavigationController>();
        _model = GameObject.Find("Player").GetComponent<PlayerModel>();
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
        if (!showingPanel && uiInput.UISelf.Pause.WasPressedThisFrame()) pauseUnPause();
    }

    public void pauseUnPause()
    {
        if (!_model.isPaused && !_model.mov || !_model.canPause) return;

        pause();
        
        _model.pauseShowed = _model.isPaused;

        if (_model.isPaused)
        {
            showPanel("PanelPausa");
            return;
        }
        
        hidePanel("PanelPausa");
        closePanel("PanelOpciones");
    }

    public void pause()
    {
        _model.isPaused = !_model.isPaused;
        Time.timeScale = _model.isPaused ? 0 : 1;
        
        navigationController.SetNavigationButtons(_model.isPaused ? pauseNavigationButtons : null);
    }

    public void closeGame()
    {
        Application.Quit();
    }

    public async void showPanel(string name)
    {
        showingPanel = true;
        GameObject panel = GameObject.Find(name);
        await panel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetUpdate(true).AsyncWaitForCompletion();
        showingPanel = false;
    }

    private NavigationButtons newNavigationButtons;

    public void StartNavigation()
    {
        navigationController.SetNavigationButtons(newNavigationButtons);
    }

    public void AddNewButtonToNavigation(GameObject button)
    {
        Buttons horizontalButtons = new Buttons();
        horizontalButtons.horizontalButtons.Add(button);
        newNavigationButtons.verticalButtons.Add(horizontalButtons);
    }

    public void SetDirectionNavigation(int dir)
    {
        newNavigationButtons.direction = 
            dir == 1 ? NavigationButtons.DirectionType.Vertical : NavigationButtons.DirectionType.Horizontal;

        if (dir == 3)
            newNavigationButtons.direction = NavigationButtons.DirectionType.All;
    }
    
    public void RestartButtonsNavigation()
    {
        newNavigationButtons = new NavigationButtons();
    }

    public void closePanel(string name)
    {
        GameObject panel = GameObject.Find(name);
        panel.transform.localScale = Vector3.zero;
    }

    public async void hidePanel(string name)
    {
        showingPanel = true;
        GameObject panel = GameObject.Find(name);
        await panel.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).SetUpdate(true)
            .AsyncWaitForCompletion();
        showingPanel = false;
    }
}
