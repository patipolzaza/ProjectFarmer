using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product : Item, ISellable
{
    public GameObject GetObject()
    {
        return gameObject;
    }

    public int GetSellPrice => ItemData.sellPrice;

    public int Sell()
    {
        Wallet playerWallet = Player.Instance.wallet;
        int price = ItemData.sellPrice;
        playerWallet.EarnCoin(price);
        Destroy(gameObject);

        return price;
    }
}
