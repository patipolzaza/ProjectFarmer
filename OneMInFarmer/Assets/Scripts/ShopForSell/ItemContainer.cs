using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer
{
    public Stack<IValuable> valuableObjects = new Stack<IValuable>();

    public int GetItemCount
    {
        get
        {
            return valuableObjects.Count;
        }
    }

    public IValuable PickItemInStash
    {
        get
        {
            if (valuableObjects.Count > 0)
            {
                return valuableObjects.Pop();
            }
            else
            {
                return null;
            }
        }
    }

    public bool Put(IValuable objectToPut)
    {
        if (objectToPut == null)
        {
            return false;
        }

        valuableObjects.Push(objectToPut);
        return true;
    }
}
