using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuyable
{
    public GameObject GetObject();

    /// <summary>
    /// Buy object that is valuable.
    /// </summary>
    /// <returns>True if success purchase, False if not.</returns>
    public bool Buy(Player player);
    public int GetBuyPrice { get; }
}
