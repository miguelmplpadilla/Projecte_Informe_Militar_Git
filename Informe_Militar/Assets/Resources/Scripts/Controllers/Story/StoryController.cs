using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XNode;

public class StoryController : MonoBehaviour
{
    public StoryCreator story;

    public Image fadeImage;

    public string[] scenesToStart;

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

        Debug.Log("Todas las escenas cargadas");
        
        StoryBaseNode firstNode = null;

        for (int i = 0; i < story.nodes.Count; i++)
        {
            if (story.nodes[i] is StartStoryNode)
            {
                firstNode = (story.nodes[i] as StartStoryNode).startStoryOutput;
                return;
            }
        }
        
        SetScene(firstNode);
    }

    private void SetNextScene(SetNextScene nextSceneData)
    {
        FadeInFadeOut(() =>
        {
            SetScene(nextSceneData.node);
        });
    }

    private void SetScene(StoryBaseNode node)
    {
        if (node is DiapositiveNode)
        {
            DiapositiveNode diapositiveNode = node as DiapositiveNode;
            FadeInFadeOut(() =>
            {
                EventBus<StartDiapositive>.Raise(new StartDiapositive
                {
                    imageDiapositive = diapositiveNode.imageDialositive,
                    backgroundDiapositive = diapositiveNode.backgroundDiapositive,
                    nextNode = diapositiveNode.next
                });
            });
        } else if (node is EndStoryNode)
        {
            // Final de juego
        }
    }

    private async void FadeInFadeOut(Action callbackFullBlack = null, Action callbackEnd = null)
    {
        fadeImage.raycastTarget = false;
        await fadeImage.DOFade(1, 2f).AsyncWaitForCompletion();
        
        callbackFullBlack?.Invoke();
        
        await fadeImage.DOFade(0, 2f).AsyncWaitForCompletion();
        fadeImage.raycastTarget = true;
        
        callbackEnd?.Invoke();
    }

    private async Task StartAllScenes()
    {
        for (int i = 0; i < scenesToStart.Length; i++)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scenesToStart[i]);
            while (!asyncOperation.isDone) await Task.Yield();
        }
    }
}
