using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;

public class ShootBulletsController : MonoBehaviour
{
    private GameObject camera;
    public GameObject warning;
    public GameObject bullets;

    private PlayerModel playerModel;

    private RunningController runningController;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        playerModel = GameObject.Find("Player").GetComponent<PlayerModel>();

        camera = GameObject.Find("MainCamera");
        runningController = GameObject.Find("RunningController").GetComponent<RunningController>();

        Shoot();
    }

    // Update is called once per frame
    void Update()
    {
        rb.DOMoveX(camera.transform.position.x, 0.1f);
    }

    private async void Shoot()
    {
        for (int i = 0; i < 10; i++)
        {
            warning.SetActive(!warning.activeSelf);
            await Task.Delay(200);
        }

        warning.SetActive(false);

        await Task.Delay(300);

        bullets.SetActive(true);

        GetComponent<BoxCollider2D>().enabled = true;

        await Task.Delay(2000);

        runningController.StartShooting();

        Destroy(gameObject);
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !playerModel.agachado) Debug.Log("Hit Player");
    }
}
