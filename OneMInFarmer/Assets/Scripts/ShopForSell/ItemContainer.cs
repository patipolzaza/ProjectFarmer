using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer
{
    public List<PickableObject> sellingObjects = new List<PickableObject>();

    public bool Put(PickableObject objectToPut)
    {
        if (objectToPut != null)
        {
            return false;
        }


        return true;
    }
}
