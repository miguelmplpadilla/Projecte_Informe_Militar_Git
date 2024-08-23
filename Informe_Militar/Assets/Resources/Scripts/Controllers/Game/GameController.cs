using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameController : BaseControllerStory
{
    public static GameController instance;
    
    private GameObject currentGame;
    public string extraArguments;

    [NonSerialized] public bool isMultiDecision;

    public GameObject parentGames;

    private StoryBaseNode primaryNextNode;
    private StoryBaseNode secondaryNextNode;

    public GameObject defaultGame;

    private bool isPlayingGame = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        EventBus<PlayGame>.Register(new EventBinding<PlayGame>(PlayGame));
        EventBus<CloseGame>.Register(new EventBinding<CloseGame>(CloseGame));
        
        EventBus<HideAllScenes>.Register(new EventBinding<HideAllScenes>(HideScene));
        EventBus<ActiveDesactiveCurrentGame>.Register(new EventBinding<ActiveDesactiveCurrentGame>(ActiveCurrentGame));
        EventBus<ReanudeGame>.Register(new EventBinding<ReanudeGame>(ReanudeGame));
    }

    private void OnDestroy()
    {
        EventBus<PlayGame>.Deregister(new EventBinding<PlayGame>(PlayGame));
        EventBus<CloseGame>.Deregister(new EventBinding<CloseGame>(CloseGame));
        
        EventBus<HideAllScenes>.Deregister(new EventBinding<HideAllScenes>(HideScene));
        EventBus<ActiveDesactiveCurrentGame>.Deregister(new EventBinding<ActiveDesactiveCurrentGame>(ActiveCurrentGame));
        EventBus<ReanudeGame>.Deregister(new EventBinding<ReanudeGame>(ReanudeGame));
    }

    private async void PlayGame(PlayGame game)
    {
        nextNode = primaryNextNode;
        
        primaryNextNode = game.primaryNextNode;
        secondaryNextNode = game.secondaryNextNode;

        isMultiDecision = primaryNextNode != null && secondaryNextNode != null;

        await Task.Yield();

        extraArguments = game.extraArguments;

        GameObject gameToInstantiate = game.gamePrefab;
        if (gameToInstantiate == null) gameToInstantiate = defaultGame;
        
        currentGame = Instantiate(gameToInstantiate, parentGames.transform);

        isPlayingGame = true;
    }

    private void CloseGame(CloseGame closeGame)
    {
        isPlayingGame = false;
        
        switch (closeGame.decision)
        {
            case 1: nextNode = primaryNextNode;
                break;
            case 2: nextNode = secondaryNextNode;
                break;
        }
        
        EventBus<SetNextScene>.Raise(new SetNextScene
        {
            node = nextNode
        });
    }

    private async void ReanudeGame(ReanudeGame r)
    {
        ActiveCurrentGame(new ActiveDesactiveCurrentGame
        {
            active = true
        });

        Task.Yield();
        Task.Yield();
        
        EventBus<SendDecision>.Raise(new SendDecision
        {
            final = r.numFinal
        });
    }

    private void ActiveCurrentGame(ActiveDesactiveCurrentGame a)
    {
        currentGame.SetActive(a.active);
    }
    
    private void HideScene()
    {
        if (isPlayingGame) return;
        
        if (currentGame != null) Destroy(currentGame);
        extraArguments = "";
    }
}
