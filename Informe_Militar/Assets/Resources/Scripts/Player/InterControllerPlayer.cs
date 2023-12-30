using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class InterControllerPlayer : MonoBehaviour
{
    public GameObject objToInter;

    public PlayerModel model;

    private CircleCollider2D circleCollider2D;

    public LayerMask layerInter;

    private PlayerControls playerControls;

    private void Awake()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();

        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private async void Update()
    {
        objToInter = comprobarInterCollider();

        if (!objToInter || !model.mov || !model.canInter) return;
        
        objToInter.SendMessage("interEnter", model);

        if (!playerControls.Gameplay.Inter.WasPressedThisFrame()) return;

        model.mov = false;
        model.canInter = false;
        objToInter.SendMessage("inter", model);
    }

    private bool checkRayCast(GameObject objCheck)
    {
        Vector2 direction = objCheck.transform.position - transform.position;

        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, layerInter);

        if (hit2D)
        {
            bool canInteract = hit2D.collider.CompareTag("Inter");
            
            Debug.DrawRay(transform.position, direction * 1000,
                !canInteract ? Color.blue : Color.green);
            
            return canInteract;
        }
        
        Debug.DrawRay(transform.position, direction * 1000, Color.red);
        
        return false;
    }

    private GameObject comprobarInterCollider()
    {
        GameObject objInter = null;

        float distanciaCercana = 10000000;

        int numObjInter = 0;

        List<Collider2D> objsInter = Physics2D.OverlapCircleAll(transform.position, circleCollider2D.radius).ToList();

        foreach (var obj in objsInter)
        {
            if (!obj.CompareTag("Inter")) continue;
            
            numObjInter++;

            float distancia = Vector3.Distance(obj.transform.position, transform.position);
            
            if (distancia > distanciaCercana || !checkRayCast(obj.gameObject)) continue;

            objInter = obj.gameObject;
            distanciaCercana = distancia;
        }

        if (objInter)
        {
            foreach (var obj in objsInter)
            {
                if (obj.gameObject.Equals(objInter) || !obj.CompareTag("Inter")) continue;

                obj.gameObject.SendMessage("interExit", model);
            }
        }

        if (numObjInter == 0) objInter = null;

        return objInter;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Inter"))
        {
            //objToInter = other.gameObject;
            //other.SendMessage("interEnter", model);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Inter"))
        {
            //objToInter = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Inter"))
            other.SendMessage("interExit", model);
    }
}