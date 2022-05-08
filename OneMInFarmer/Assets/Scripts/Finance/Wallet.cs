using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour, IContainStatus
{
    private WalletSaveData _saveData;

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

    private void Start()
    {
        _saveData = new WalletSaveData(this);
        UpdateSaveData();
    }

    public void LoadSaveData(WalletSaveData saveData)
    {
        coin = saveData.GetCoinInWallet;
        _bonusCoinStatus.SetLevel(saveData.GetWalletStatusLevel);

        _saveData = saveData;
    }

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

    private void SetCoin(int amount)
    {
        int oldValue = coin;
        coin = Mathf.Clamp(coin, 0, amount);
        OnCoinChanged.Invoke(oldValue, coin);
    }

    public void UpdateSaveData()
    {
        _saveData.UpdateData(this);
    }
}
