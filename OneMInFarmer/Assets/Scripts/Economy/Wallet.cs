using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet
{
    public int coin { get; private set; }

    public Wallet(int initialCoin)
    {
        coin = initialCoin;
    }

    public void EarnCoin(int amount)
    {
        coin += amount;
    }

    public void LoseCoin(int amount)
    {
        if (coin <= amount)
        {
            coin = 0;
        }
        else
        {
            coin -= amount;
        }
    }
}
