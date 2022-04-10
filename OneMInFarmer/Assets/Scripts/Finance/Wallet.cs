using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    public delegate void CoinChangedDelegate(int oldValue, int newValue);
    public CoinChangedDelegate OnCoinChanged;

    public int coin { get; private set; }

    private void Awake()
    {
        coin = 10;
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
