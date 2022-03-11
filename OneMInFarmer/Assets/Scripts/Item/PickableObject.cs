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

    public virtual void PickUp(Player player)
    {
        player.SetHoldingItem(this);
    }

    public virtual void Drop(Player player)
    {
        player.SetHoldingItem(null);
    }
}
