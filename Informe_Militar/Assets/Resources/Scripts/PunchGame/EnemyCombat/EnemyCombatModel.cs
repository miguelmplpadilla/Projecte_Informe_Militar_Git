using UnityEngine;

public class EnemyCombatModel : MonoBehaviour
{
    public bool canHit = true;
    public bool canMove = true;

    public Rigidbody2D rb;
    public Animator animator;

    public GameObject player;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        player = GameObject.Find("PlayerCombat");
    }
}