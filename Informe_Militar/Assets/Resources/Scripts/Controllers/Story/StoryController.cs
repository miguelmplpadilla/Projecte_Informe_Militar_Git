using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StoryController : MonoBehaviour
{
    public StoryCreator story;

    public string[] scenesToStart;

    public GameObject canvasPortada;

    private bool isPlayingNodeInGame = false;

    private void Awake()
    {
        EventBus<SetNextScene>.Register(new EventBinding<SetNextScene>(SetNextScene));
        EventBus<PlayNodeInGame>.Register(new EventBinding<PlayNodeInGame>(PlayNodeInGame));
    }
    
    private void OnDestroy()
    {
        EventBus<SetNextScene>.Deregister(new EventBinding<SetNextScene>(SetNextScene));
        EventBus<PlayNodeInGame>.Deregister(new EventBinding<PlayNodeInGame>(PlayNodeInGame));
    }

    private async void Start()
    {
        await StartAllScenes();

        await Task.Yield();
        
        DestroyCamerasAndEventSystems();
        
        StoryBaseNode firstNode = null;

        for (int i = 0; i < story.nodes.Count; i++)
        {
            if (story.nodes[i] is StartStoryNode)
            {
                firstNode = (story.nodes[i] as StartStoryNode).startStoryOutput;
                break;
            }
        }
        
        EventBus<FadeInFadeOut>.Raise(new FadeInFadeOut
        {
            fade = true,
            callback = () =>
            {
                canvasPortada.SetActive(false);
                SetScene(firstNode);
            }
        });
    }

    private void SetNextScene(SetNextScene nextSceneData)
    {
        EventBus<FadeInFadeOut>.Raise(new FadeInFadeOut
        {
            fade = true,
            callback = async () =>
            {
                EventBus<HideAllScenes>.Raise(new HideAllScenes());
                
                await Task.Delay(100);
                
                if (isPlayingNodeInGame)
                {
                    EventBus<ReanudeGame>.Raise(new ReanudeGame
                    {
                        numFinal = nextSceneData.node == null ? 0 :
                            nextSceneData.node.name.Equals("PrimaryFinalGame") ? 1 : 2
                    });
                    
                    EventBus<FadeInFadeOut>.Raise(new FadeInFadeOut
                    {
                        fade = false,
                        callback = () =>
                        {
                            isPlayingNodeInGame = false;
                        }
                    });
                    return;
                }
                
                SetScene(nextSceneData.node);
            }
        });
    }

    private async void SetScene(StoryBaseNode node)
    {
        await Task.Delay(700);
        
        PlayNodeType(node);
    }

    private void PlayNodeType(StoryBaseNode node)
    {
        if (node is DiapositiveNode)
        {
            DiapositiveNode diapositiveNode = node as DiapositiveNode;
            EventBus<StartDiapositive>.Raise(new StartDiapositive
            {
                imageDiapositiveFront = diapositiveNode.dataDiapositive.frontES,
                imageDiapositiveBack = diapositiveNode.dataDiapositive.backES,
                backgroundDiapositive = diapositiveNode.dataDiapositive.backgroundDiapositive,
                descriptionFront = diapositiveNode.dataDiapositive.descriptionFront,
                descriptionBack = diapositiveNode.dataDiapositive.descriptionBack,
                textFront = diapositiveNode.dataDiapositive.textFront,
                textBack = diapositiveNode.dataDiapositive.textBack,
                nextNode = diapositiveNode.next
            });
            EventBus<FadeInFadeOut>.Raise(new FadeInFadeOut
            {
                fade = false
            });
        } else if (node is AnimationNode)
        {
            AnimationNode animationNode = node as AnimationNode;
            EventBus<SetAnimation>.Raise(new SetAnimation
            {
                video = animationNode.videoAnimationES,
                descriptionAnimation = animationNode.description,
                nextNode = animationNode.next
            });
        } else if (node is GameNode)
        {
            GameNode gameNode = node as GameNode;
            EventBus<PlayGame>.Raise(new PlayGame
            {
                gamePrefab = gameNode.prefabGame,
                extraArguments = gameNode.extraArguments,
                primaryNextNode = gameNode.primaryNext,
                secondaryNextNode = gameNode.secondaryNext
            });
            EventBus<FadeInFadeOut>.Raise(new FadeInFadeOut
            {
                fade = false
            });
        } else if (node is DialogueStoryNode)
        {
            DialogueStoryNode dialogueStoryNode = node as DialogueStoryNode;
            DialogueCreator dialogueCreator = dialogueStoryNode.dialogueGraph;

            DialogueBaseNode startDialogueNode = null;

            for (int i = 0; i < dialogueCreator.nodes.Count; i++)
                if (dialogueCreator.nodes[i] is StartDialogueNode)
                {
                    startDialogueNode = (dialogueCreator.nodes[i] as StartDialogueNode).dialogueStart;
                    break;
                }
            
            EventBus<SetDialogue>.Raise(new SetDialogue
            {
                primaryEnd = dialogueStoryNode.primaryNext,
                secondaryEnd = dialogueStoryNode.secondaryNext,
                startDialogue = startDialogueNode,
                imageBackground = dialogueStoryNode.background
            });
        }
    }

    private void PlayNodeInGame(PlayNodeInGame p)
    {
        isPlayingNodeInGame = true;
        
        EventBus<FadeInFadeOut>.Raise(new FadeInFadeOut
        {
            fade = true,
            callback = () =>
            {
                EventBus<ActiveDesactiveCurrentGame>.Raise(new ActiveDesactiveCurrentGame
                {
                    active = false
                });
                
                PlayNodeType(p.node);
            }
        });
    }

    private async Task StartAllScenes()
    {
        for (int i = 0; i < scenesToStart.Length; i++)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scenesToStart[i], LoadSceneMode.Additive);
            while (!asyncOperation.isDone) await Task.Yield();
        }
        
        Debug.Log("Todas las escenas cargadas");
    }

    private void DestroyCamerasAndEventSystems()
    {
        EventSystem[] allEventSystems = FindObjectsOfType<EventSystem>();
        Camera[] allCameras = FindObjectsOfType<Camera>();

        for (int i = 0; i < allEventSystems.Length; i++)
            if (!allEventSystems[i].name.Equals("MainEventSystem"))
                Destroy(allEventSystems[i].gameObject);
        
        for (int i = 0; i < allCameras.Length; i++)
            if (!allCameras[i].name.Equals("MainCamera"))
                Destroy(allCameras[i].gameObject);
    }
}
