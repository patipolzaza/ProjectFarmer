using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour, IContainStatus
{
    private Status _bonusCoinStatus;
    [SerializeField] private StatusData _bonusCoinStatusData;

    public delegate void CoinChangedDelegate(int oldValue, int newValue);
    public CoinChangedDelegate OnCoinChanged;

    public int coin { get; private set; } = 10;


    private void Awake()
    {
        _bonusCoinStatus = new Status("Bonus Coin", _bonusCoinStatusData);
    }

    public Status GetStatus => _bonusCoinStatus;

    public void EarnCoin(int amount)
    {
        int oldCoinValue = coin;
        coin += (amount + _bonusCoinStatus.GetValue);
        OnCoinChanged?.Invoke(oldCoinValue, coin);
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
    }
}
