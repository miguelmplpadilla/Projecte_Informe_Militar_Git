using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorExecuter : MonoBehaviour
{
    private PlayerModel _model;

    private void Start()
    {
        _model = transform.GetComponent<PlayerModel>();
    }

    public void setSlidingFalse()
    {
        _model.sliding = false;
    }
}