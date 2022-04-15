using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : Item, IBuyable, IUsable
{
    public int GetBuyPrice => ItemData.purchasePrice;

    private void OnValidate()
    {
        if (ItemData != null && ItemData is SeedData == false)
        {
            ItemData = null;
        }
    }

    public bool Buy(Player player)
    {
        Wallet playerWallet = player.wallet;
        int price = GetBuyPrice;

        if (playerWallet.coin >= price)
        {
            playerWallet.LoseCoin(price);
            return true;
        }
        else
        {
            return false;
        }
    }

    public GameObject GetObject() => gameObject;

    public bool Use(Interactable targetToUse)
    {
        if (targetToUse is Plot)
        {
            Plot plot = (Plot)targetToUse;
            if (plot.Plant(ItemData as SeedData))
            {
                currentStack--;

                if (currentStack <= 0)
                {
                    Destroy(gameObject);
                    return true;
                }
            }
        }

        return false;
    }
}
