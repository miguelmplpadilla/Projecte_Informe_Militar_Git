using UnityEngine;

public class EnemyCombatModel : MonoBehaviour
{
    public bool canHit = true;
    public bool canMove = true;
    public bool dashing = false;

    public bool atack = false;
    public bool atacking = false;

    public float timeWaitAtack = 1;
    public float currentTimeAtack = 0;

    public float dashSpeed = 1;

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