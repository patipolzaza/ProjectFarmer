using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISellable
{
    public GameObject GetObject();

    /// <summary>
    /// Function for sell valuable object
    /// </summary>
    /// <returns>Price that valuable object was sold.</returns>
    public int Sell();
    public int GetSellPrice { get; }
    public Sprite GetIcon { get; }
}
