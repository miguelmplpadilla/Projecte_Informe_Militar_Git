using System;
using DG.Tweening;
using Resources.Scripts.Controllers.Player;
using UnityEngine;

[RequireComponent(typeof(PlayerModel))]
public class PlayerMovement3D : MonoBehaviour
{
    public static PlayerMovement3D instance;
    
    public float currentSpeed = 0;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    [NonSerialized] public bool isCrouched = false;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Animator animator;

    public float maxVelocityCrouch = 5;
    public float maxVelocityWalk = 6;
    public float maxVelocityRun = 10;
    
    public float maxVelocityGlobal = 10;
    
    private float lastMaxVelocity = 0;

    public GameObject center;

    [SerializeField] private float distanceCheckWall = 0.3f;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        maxVelocityGlobal = maxVelocityRun;
    }
    
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = -4f;

        float maxVelocity = maxVelocityWalk;
        if (isCrouched) maxVelocity = maxVelocityCrouch;
        
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouched) maxVelocity = maxVelocityRun;

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if (x == 0 && z == 0)
        {
            maxVelocity = 0;
            if (currentSpeed > maxVelocityWalk) currentSpeed = 0;
        }
        
        if (currentSpeed < maxVelocity) currentSpeed += 0.1f;
        if (currentSpeed > maxVelocity) currentSpeed -= 0.1f;
        
        float finalVelocity = 0;
        
        if (PlayerModel.instance.canMove)
        {
            Vector3 cameraForward = new Vector3(CameraManager.instance.transform.forward.x, 0, CameraManager.instance.transform.forward.z).normalized;
            Vector3 cameraRight = new Vector3(CameraManager.instance.transform.right.x, 0, CameraManager.instance.transform.right.z).normalized;
            Vector3 move = cameraRight * x + cameraForward * z;
            controller.Move(move * currentSpeed * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.C))
            {
                isCrouched = !isCrouched;
                animator.SetBool("crouched", isCrouched);
                animator.SetTrigger(isCrouched ? "crouch" : "up");
                maxVelocityGlobal = isCrouched ? maxVelocityCrouch : maxVelocityRun;
            }
            
            if (move != Vector3.zero)
                animator.transform.DORotateQuaternion(Quaternion.LookRotation(move), 0.4f);
            
            finalVelocity = currentSpeed / maxVelocityGlobal;
            if (CheckWall()) finalVelocity = 0;
        }
        
        animator.SetFloat("velocity", finalVelocity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (lastMaxVelocity > 0  && maxVelocity == 0)
        {
            OnEndRun();
        }

        lastMaxVelocity = maxVelocity;
    }

    private void OnEndRun()
    {
        if (currentSpeed > maxVelocityWalk) animator.SetTrigger("EndRun");
    }

    private bool CheckWall()
    {
        RaycastHit[] hits = Physics.RaycastAll(center.transform.position, center.transform.forward, distanceCheckWall);

        Debug.DrawRay(center.transform.position, center.transform.forward * distanceCheckWall, Color.red);

        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall") || 
                hit.collider.gameObject.layer == LayerMask.NameToLayer("NormalWall")) return true;
        }

        return false;
    }
}