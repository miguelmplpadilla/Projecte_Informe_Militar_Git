using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterAllManager : MonoBehaviour
{
    public static void hideAllInterButtons()
    {
        foreach (var buttonInter in FindObjectsOfType<ButtonInterController>())
            buttonInter.esconder();
    }
}
