using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Seed : Item, IBuyable, IUsable
{
    public int GetBuyPrice => ((SeedData)ItemData).purchasePrice;
    public Sprite GetIcon => ItemData.Icon;
    public GameObject GetObject() => gameObject;
    public Product GetProduct => ((SeedData)ItemData).product;

    private void OnValidate()
    {
        if (ItemData != null && ItemData is SeedData == false)
        {
            ItemData = null;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        AddTargetType(typeof(Plot));
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

    public void AddTargetType(Type targetType)
    {
        ItemUseMatcher.AddUseItemPair(GetType(), targetType);
    }
}
