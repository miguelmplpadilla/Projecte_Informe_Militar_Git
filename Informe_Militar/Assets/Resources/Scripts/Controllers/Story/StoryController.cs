using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class StoryController : MonoBehaviour
{
    public StoryCreator story;

    public Image fadeImage;

    public string[] scenesToStart;

    public GameObject canvasPortada;

    private void Awake()
    {
        EventBus<SetNextScene>.Register(new EventBinding<SetNextScene>(SetNextScene));
    }
    
    private void OnDestroy()
    {
        EventBus<SetNextScene>.Deregister(new EventBinding<SetNextScene>(SetNextScene));
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
        
        FadeIn(() =>
        {
            canvasPortada.SetActive(false);
            SetScene(firstNode);
        });
    }

    private void SetNextScene(SetNextScene nextSceneData)
    {
        FadeIn(async () =>
        {
            EventBus<HideAllScenes>.Raise(new HideAllScenes());

            await Task.Delay(100);
            
            SetScene(nextSceneData.node);
        });
    }

    private async void SetScene(StoryBaseNode node)
    {
        await Task.Delay(700);
        
        if (node is DiapositiveNode)
        {
            DiapositiveNode diapositiveNode = node as DiapositiveNode;
            EventBus<StartDiapositive>.Raise(new StartDiapositive
            {
                imageDiapositive = diapositiveNode.imageDialositiveES,
                backgroundDiapositive = diapositiveNode.backgroundDiapositive,
                description = diapositiveNode.description,
                nextNode = diapositiveNode.next
            });
            FadeOut();
        } else if (node is AnimationNode)
        {
            AnimationNode animationNode = node as AnimationNode;
            EventBus<SetAnimation>.Raise(new SetAnimation
            {
                video = animationNode.videoAnimationES,
                descriptionAnimation = animationNode.description,
                nextNode = animationNode.next
            });
            EventBus<PlayAnimation>.Raise(new PlayAnimation());
            
            FadeOut();
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
            FadeOut();
        }
    }

    private async void FadeIn(Action callback = null)
    {
        await fadeImage.DOFade(1, 2f).AsyncWaitForCompletion();
        fadeImage.transform.parent.GetComponent<GraphicRaycaster>().enabled = false;
        
        callback?.Invoke();
    }
    
    private async void FadeOut(Action callback = null)
    {
        await fadeImage.DOFade(0, 2f).AsyncWaitForCompletion();
        fadeImage.transform.parent.GetComponent<GraphicRaycaster>().enabled = true;
        
        callback?.Invoke();
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
