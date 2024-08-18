using System;
using UnityEngine;

public class SelectorButton : MonoBehaviour
{
    public string selectorType = "Head";
    public string skin = "Normal";

    private NPCAnimatorController npcAnimatorController;

    private void Start()
    {
        npcAnimatorController = GameObject.Find("NPC").GetComponent<NPCAnimatorController>();
    }

    public void changeSprite()
    {
        switch (selectorType)
        {
            case "Head":
                npcAnimatorController.character.idHead = skin;
                break;
            case "Torso":
                npcAnimatorController.character.idTorso = skin;
                break;
            case "Legs":
                npcAnimatorController.character.idLegs = skin;
                break;
            case "Feet":
                npcAnimatorController.character.idFeets = skin;
                break;
            case "Face":
                npcAnimatorController.character.idFace = skin;
                break;
        }
    }
}
