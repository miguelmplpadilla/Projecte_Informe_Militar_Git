using System.Collections;
using UnityEngine;

public class ObjPickUp : InteractBaseController
{
    public TypePickUp typePickUp;
    public enum TypePickUp
    {
        UP, GROUND
    }
    
    protected override IEnumerator Inter()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
