using System;
using UnityEngine;

public class DefaultGameController : MonoBehaviour
{
    public GameObject oneButton;
    public GameObject optionsButtons;
    
    private void Start()
    {
        EventBus<SendDecision>.Register(new EventBinding<SendDecision>(ReciveDecision));
        
        if (GameController.instance.isMultiDecision)
        {
            oneButton.SetActive(false);
            optionsButtons.SetActive(true);
            return;
        }
        
        oneButton.SetActive(true);
        optionsButtons.SetActive(false);
    }

    private void OnDestroy()
    {
        EventBus<SendDecision>.Deregister(new EventBinding<SendDecision>(ReciveDecision));
    }

    public void CloseGame(int decisionNumber = 0)
    {
        EventBus<CloseGame>.Raise(new CloseGame
        {
            decision = decisionNumber
        });
    }

    private void ReciveDecision(SendDecision s)
    {
        Debug.Log("Decision: "+s.final);
    }

    public void PlayButton(StoryBaseNode node)
    {
        EventBus<PlayNodeInGame>.Raise(new PlayNodeInGame
        {
            node = node
        });
    }
}
