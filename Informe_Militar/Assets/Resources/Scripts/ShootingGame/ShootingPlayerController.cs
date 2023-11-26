using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ShootingPlayerController : MonoBehaviour
{
    public bool pointing = false;

    public LayerMask enemys;

    private GameObject playerCamera;

    private void Start()
    {
        playerCamera = GameObject.Find("MainCamera");
    }

    void Update()
    {
        pointing = false;
        if (Input.GetKey(KeyCode.Mouse1))
            pointing = true;

        transform.GetChild(0).gameObject.SetActive(!pointing);
        transform.GetChild(2).gameObject.SetActive(pointing);

        playerCamera.transform.DOLocalMoveZ(pointing ? 1.5f : 0, 0.2f);

        if (pointing && Input.GetKeyDown(KeyCode.Mouse0)) Shoot();
    }

    private async void Shoot()
    {
        ShowFlash();

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit,
            Mathf.Infinity, enemys))
        {
            await Task.Delay(100);
            Destroy(hit.collider.gameObject);
        }
    }

    private async void ShowFlash()
    {
        transform.GetChild(1).gameObject.SetActive(true);
        await Task.Delay(100);
        transform.GetChild(1).gameObject.SetActive(false);
    }
}
