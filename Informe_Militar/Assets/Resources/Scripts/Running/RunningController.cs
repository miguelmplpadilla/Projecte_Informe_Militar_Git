using UnityEngine;
using UnityEngine.UI;

public class RunningController : MonoBehaviour
{
    public GameObject ground;

    private GameObject player;

    private Vector2 lastPositionPlayer;
    private Vector2 startPoint;

    public GameObject[] prefabsAlly;
    public GameObject[] shootTypesPrefabs;

    public float distanceToCreate = 15;
    public float distanceToStart = 200;

    public int minTimeStartShooting = 5;
    public int maxTimeStartShooting = 10;

    public bool endGame = false;
    private bool canCreateAlly = true;

    public int cantAlyesToHelp = 10;
    public int cantAlyesHelped = 0;

    private Scrollbar lineEndGame;

    private void Start()
    {
        lineEndGame = GameObject.Find("LineEndGame").GetComponent<Scrollbar>();
        player = GameObject.Find("Player");

        lastPositionPlayer = player.transform.position;
        startPoint = lastPositionPlayer;

        StartShooting();
    }

    public void StartShooting()
    {
        Invoke("ShootPlayer", Random.Range(minTimeStartShooting, maxTimeStartShooting));
    }

    void Update()
    {
        lineEndGame.value = (float) cantAlyesHelped / cantAlyesToHelp;

        if (player.transform.position.x < startPoint.x) return;

        if (player.transform.position.x < lastPositionPlayer.x)
            ground.transform.position = new Vector2(player.transform.position.x, ground.transform.position.y);

        if (endGame) return;

        ground.transform.position = 
            new Vector2(player.transform.position.x, ground.transform.position.y);

        float distance = Vector2.Distance(player.transform.position, lastPositionPlayer);

        if (player.transform.position.x < lastPositionPlayer.x || distance < distanceToCreate || !canCreateAlly) return;

        CreateAlly();
    }

    private void CreateAlly()
    {
        GameObject allyGenerated = Instantiate(prefabsAlly[Random.Range(0, prefabsAlly.Length)],  new Vector3(player.transform.position.x + 11, -3.6f, 0), Quaternion.identity);

        lastPositionPlayer = allyGenerated.transform.position;

        SetCanCreateAlly(false);
    }

    private void ShootPlayer()
    {
        if (endGame) return;

        Instantiate(shootTypesPrefabs[Random.Range(0, shootTypesPrefabs.Length)]);
    }

    public void SetCanCreateAlly(bool can, bool sum = true)
    {
        if (!sum)
        {
            canCreateAlly = can;
            return;
        }
           
        if (can) cantAlyesHelped++;

        if (cantAlyesHelped >= cantAlyesToHelp + 1) endGame = true;

        canCreateAlly = can;
    }

    public void RestAllyHelped()
    {
        cantAlyesHelped--;
    }
}
