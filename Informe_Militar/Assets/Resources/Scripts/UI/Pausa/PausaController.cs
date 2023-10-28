using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PausaController : MonoBehaviour
{
    private PlayerModel _model;
    private GameObject panelPause;

    private bool showingPanel = false;

    private void Start()
    {
        _model = GameObject.Find("Player").GetComponent<PlayerModel>();
    }

    private void Update()
    {
        if (!showingPanel && Input.GetKeyDown(KeyCode.Escape)) pauseUnPause();
    }

    public void pauseUnPause()
    {
        if (!_model.isPaused && !_model.mov) return;
        
        _model.isPaused = !_model.isPaused;
        Time.timeScale = _model.isPaused ? 0 : 1;

        if (_model.isPaused)
        {
            showPanel("PanelPausa");
        }
        else
        {
            hidePanel("PanelPausa", false);
            hidePanel("PanelOpciones", true);
        }
    }

    public void closeGame()
    {
        Application.Quit();
    }

    public async void showPanel(string name)
    {
        showingPanel = true;
        GameObject panel = GameObject.Find(name);
        await panel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetUpdate(true).AsyncWaitForCompletion();
        showingPanel = false;
    }

    public async void hidePanel(string name, bool fast)
    {
        showingPanel = true;
        GameObject panel = GameObject.Find(name);
        if (!fast)
            await panel.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).SetUpdate(true)
                .AsyncWaitForCompletion();
        else
            panel.transform.localScale = Vector3.zero;
        showingPanel = false;
    }
}
