using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Item : MonoBehaviour
{
    public ItemData ItemData;

    public void PickUp(Player player)
    {
        player.SetHoldingItem(this);
    }

    public void Drop(Player player)
    {
        player.SetHoldingItem(null);
    }
}
