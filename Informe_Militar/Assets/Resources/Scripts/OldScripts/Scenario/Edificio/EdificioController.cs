using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdificioController : MonoBehaviour
{
    public bool entering = false;
    public bool playerOnCollider = false;
    public bool playerOnBuilding = false;

    private SpriteRenderer puertaSr;
    public SpriteRenderer fachadaSr;
    public GameObject interior;

    private void Awake()
    {
        puertaSr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        int numVerticalCompare = !playerOnBuilding ? 1 : -1;
        transform.GetChild(0).SendMessage("SetKey", numVerticalCompare == 1 ? KeyCode.W : KeyCode.S);
        if (!playerOnCollider || entering || Input.GetAxisRaw("Vertical") != numVerticalCompare)
            return;

        SetPlayerOnBuilding(!playerOnBuilding);
    }

    public void SetPlayerOnBuilding(bool enter)
    {
        entering = true;

        puertaSr.sortingOrder = enter ? 4 : 0;

        Color colorFachada = fachadaSr.color;
        colorFachada.a = enter ? 0:1;

        if (!enter) fachadaSr.gameObject.SetActive(true);
        else interior.SetActive(true);
        fachadaSr.DOColor(colorFachada, 0.3f).OnComplete(() =>
        {
            if (enter) fachadaSr.gameObject.SetActive(false);
            else interior.SetActive(false);

            entering = false;
        });
        

        playerOnBuilding = enter;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        transform.GetChild(0).SendMessage("mostrar");
        playerOnCollider = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        transform.GetChild(0).SendMessage("esconder");
        playerOnCollider = false;
    }
}
