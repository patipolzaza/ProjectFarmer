using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet
{
    public delegate void CoinChangedDelegate(int oldValue, int newValue);
    public CoinChangedDelegate OnCoinChanged;

    public int coin { get; private set; }

    public Wallet(int initialCoin)
    {
        coin = initialCoin;
    }

    public void EarnCoin(int amount)
    {
        int oldCoinValue = coin;
        coin += amount;
        OnCoinChanged?.Invoke(oldCoinValue, coin);
        //WalletUI.Instance.UpdateCoinText(coin);
    }

    public void LoseCoin(int amount)
    {
        int oldCoinValue = coin;
        if (coin <= amount)
        {
            coin = 0;
        }
        else
        {
            coin -= amount;
        }

        OnCoinChanged?.Invoke(oldCoinValue, coin);
        //WalletUI.Instance.UpdateCoinText(coin);
    }
}
