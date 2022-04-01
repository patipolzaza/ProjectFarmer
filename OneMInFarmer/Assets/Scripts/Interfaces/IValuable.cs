using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IValuable
{
    /// <summary>
    /// Purchase valuable object from shop.
    /// </summary>
    /// <returns>True if success purchase, False if not.</returns>
    public bool Purchase(); // Return bool that is finished purchase.
    /// <summary>
    /// Function for sell valuable object
    /// </summary>
    /// <returns>Price that valuable object was sold.</returns>
    public int Sell();
    public GameObject GetObject();
}
