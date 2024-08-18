using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class ShootMortar : MonoBehaviour
{
    private GameObject player;
    public GameObject bullet;
    public GameObject warning;

    private CircleCollider2D circleColliderBullet;
    private Rigidbody2D rb;

    private bool followPlayer = true;

    private RunningController runningController;

    private void Awake()
    {
        circleColliderBullet = bullet.GetComponent<CircleCollider2D>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        player = GameObject.Find("Player");
        runningController = GameObject.Find("RunningController").GetComponent<RunningController>();

        StartShooting();
    }

    private void Update()
    {
        if (!followPlayer) return;

        rb.DOMoveX(player.transform.position.x, 0.1f);
    }

    private async void StartShooting()
    {
        for (int i = 0; i < Random.Range(3, 5 + 1) * 10; i++)
        {
            warning.SetActive(!warning.activeSelf);
            await Task.Delay(100);
        }

        warning.SetActive(false);

        Shoot();
    }

    private void Shoot()
    {
        followPlayer = false;

        bullet.transform.DOMove(player.transform.position, 0.7f).SetEase(Ease.Linear).OnComplete(() =>
        {
            List<Collider2D> colliders = new List<Collider2D>();
            circleColliderBullet.OverlapCollider(new ContactFilter2D(), colliders);

            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Player")) Debug.Log("Player Hit");
                if (collider.CompareTag("Ally")) Destroy(collider.gameObject);
            }

            runningController.StartShooting();

            Destroy(gameObject);
        });
    }
}
