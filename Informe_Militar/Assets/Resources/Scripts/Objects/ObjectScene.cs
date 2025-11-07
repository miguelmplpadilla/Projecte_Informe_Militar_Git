using System.Collections;
using UnityEngine;

public class ObjectScene : ObjPickUp
{
    public int cantItem = 1;

    protected override IEnumerator Inter()
    {
        yield return new WaitForSeconds(1);
        InventoryManager.instance.AddObjectToInventory(key, cantItem);
        Destroy(gameObject);
    }
}
