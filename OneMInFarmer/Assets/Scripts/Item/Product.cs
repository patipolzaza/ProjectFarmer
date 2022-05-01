using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product : Item, ISellable
{
    public GameObject GetObject()
    {
        return gameObject;
    }

    public int GetSellPrice => ((ProductData)ItemData).sellPrice;
    public Sprite GetIcon => ItemData.Icon;

    public int Sell()
    {
        Wallet playerWallet = Player.Instance.wallet;
        int price = GetSellPrice;
        playerWallet.EarnCoin(price);
        Destroy(gameObject);

        return price;
    }
}
