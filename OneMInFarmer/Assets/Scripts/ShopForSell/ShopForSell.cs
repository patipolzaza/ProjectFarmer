using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopForSell : Interactable
{
    private ItemContainer itemContainer;

    private void Start()
    {

    }

    public bool PutItemInContainer(Player player)
    {
        if (player && player.holdingObject /*is Valuable*/)
        {
            
        }

        return true;
    }
}
