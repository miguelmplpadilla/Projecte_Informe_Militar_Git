using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorExecuter : MonoBehaviour
{
    private PlayerModelDeprecated modelDeprecated;

    private void Start()
    {
        modelDeprecated = transform.GetComponent<PlayerModelDeprecated>();
    }

    public void setSlidingFalse()
    {
        modelDeprecated.sliding = false;
    }
}