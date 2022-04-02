using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet
{
    public int coin { get; private set; }

    public Wallet(int initialCoin)
    {
        coin = initialCoin;
        WalletUI.Instance.SetCoinText(coin);
    }

    public void EarnCoin(int amount)
    {
        coin += amount;
        WalletUI.Instance.SetCoinText(coin);
    }

    public bool LoseCoin(int amount)
    {
        if (coin <= amount)
        {
            coin = 0;
            return false;
        }
        else
        {
            coin -= amount;
            WalletUI.Instance.SetCoinText(coin);
            return true;
        }
    }
}
