using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjacentDoorController : MonoBehaviour
{
    public EdificioController edificioController;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            edificioController.SetPlayerOnBuilding(false);

    }
}
