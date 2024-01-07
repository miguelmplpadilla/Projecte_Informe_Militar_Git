using UnityEngine;

public class EnemyCombatModel : MonoBehaviour
{
    public bool canHit = true;
    public bool canMove = true;

    public Rigidbody2D rb;
    public Animator animator;

    public GameObject player;

    public float life = 10;
    public float maxLife = 10;

    private void Awake()
    {
        life = maxLife;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        player = GameObject.Find("PlayerCombat");
    }
}