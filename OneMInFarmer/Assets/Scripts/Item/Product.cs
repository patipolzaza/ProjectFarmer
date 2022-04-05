using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product : Item, IValuable
{
    public GameObject GetObject()
    {
        throw new System.NotImplementedException();
    }

    public bool Purchase()
    {
        throw new System.NotImplementedException();
    }

    public int Sell()
    {
        Wallet playerWallet = Player.Instance.wallet;
        int price = ItemData.sellPrice;
        playerWallet.EarnCoin(price);
        Destroy(gameObject);

        return price;
    }
}
