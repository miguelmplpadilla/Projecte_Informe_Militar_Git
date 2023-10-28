using System;
using System.Collections;
using System.Collections.Generic;
using Resources.Scripts;
using UnityEngine;

public class LaderController : MonoBehaviour, InterBaseInterface
{
    private GameObject _button;
    private GameObject _headPlayer;
    private GameObject _player;
    private Animator _playerAnimator;

    public bool playerClimbing = false;

    private void Start()
    {
        _button = transform.GetChild(0).GetChild(0).gameObject;
        _player = GameObject.Find("Player");
        _headPlayer = _player.transform.Find("Head").gameObject;
    }

    private void Update()
    {
        _button.transform.position = new Vector3(_button.transform.position.x, _headPlayer.transform.position.y,
            _button.transform.position.z);

        if (!playerClimbing) return;
        
        
    }

    public void interEnter(PlayerModel model)
    {
    }

    public void inter(PlayerModel model)
    {
        _player.transform.position = new Vector3(transform.position.x, _player.transform.position.y);
        _playerAnimator.SetTrigger("climb");
        _playerAnimator.SetBool("climbing", true);
        _playerAnimator.StopPlayback();
        playerClimbing = true;
    }

    public void interExit(PlayerModel model)
    {
    }
}
