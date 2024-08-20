using System;
using UnityEngine;

public class DefaultGameController : MonoBehaviour
{
    public GameObject oneButton;
    public GameObject optionsButtons;
    
    private void Start()
    {
        Debug.Log(GameController.instance.isMultiDecision);
        if (GameController.instance.isMultiDecision)
        {
            oneButton.SetActive(false);
            optionsButtons.SetActive(true);
            return;
        }
        
        oneButton.SetActive(true);
        optionsButtons.SetActive(false);
    }

    public void CloseGame(int decisionNumber = 0)
    {
        EventBus<CloseGame>.Raise(new CloseGame
        {
            decision = decisionNumber
        });
    }
}
