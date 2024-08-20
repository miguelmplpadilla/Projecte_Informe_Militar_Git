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

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        EventBus<PlayGame>.Register(new EventBinding<PlayGame>(PlayGame));
        EventBus<CloseGame>.Register(new EventBinding<CloseGame>(CloseGame));
        
        EventBus<HideAllScenes>.Register(new EventBinding<HideAllScenes>(HideScene));
    }

    private void OnDestroy()
    {
        EventBus<PlayGame>.Deregister(new EventBinding<PlayGame>(PlayGame));
        EventBus<CloseGame>.Deregister(new EventBinding<CloseGame>(CloseGame));
        
        EventBus<HideAllScenes>.Deregister(new EventBinding<HideAllScenes>(HideScene));
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
    }

    private void CloseGame(CloseGame closeGame)
    {
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
    
    private void HideScene()
    {
        if (currentGame != null) Destroy(currentGame);
        extraArguments = "";
    }
}
