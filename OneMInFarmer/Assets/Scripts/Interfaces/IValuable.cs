using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IValuable
{
    public bool Purchase();
    public void Sell();
    public GameObject GetObject();
}
