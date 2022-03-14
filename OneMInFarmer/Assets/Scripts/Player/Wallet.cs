using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet
{
    public int currentCoin { get; private set; }

    public Wallet(int initialCoin)
    {
        currentCoin = initialCoin;
    }

    public void EarnCoin(int amount)
    {
        currentCoin += amount;
    }

    public void LoseCoin(int amount)
    {
        if (currentCoin <= amount)
        {
            currentCoin = 0;
        }
        else
        {
            currentCoin -= amount;
        }
    }
}
