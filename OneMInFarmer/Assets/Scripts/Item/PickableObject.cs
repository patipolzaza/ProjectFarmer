using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : Interactable
{

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
