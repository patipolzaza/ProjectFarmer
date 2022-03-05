using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Item : Interactable
{
    public ItemData ItemData;

    protected override void Awake()
    {
        base.Awake();

        interactEvent.AddListener(PickUp);
    }

    public void PickUp(Player player)
    {
        player.SetHoldingItem(this);
    }

    public void Drop(Player player)
    {
        player.SetHoldingItem(null);
    }

}
