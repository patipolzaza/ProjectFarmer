using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Wallet : MonoBehaviour, IContainStatus
{
    private WalletSaveData _saveData;

    private Status _bonusCoinStatus;
    [SerializeField] private StatusData _bonusCoinStatusData;

    public delegate void CoinChangedDelegate(int oldValue, int newValue);
    public CoinChangedDelegate OnCoinChanged;

    public delegate void CoinSettedDelegate(int settedValue);
    public CoinSettedDelegate OnCoinSetted;

    public int coin { get; private set; } = 10;


    private void Awake()
    {
        _bonusCoinStatus = new Status("Bonus Coin", _bonusCoinStatusData);
    }

    public Status GetStatus => _bonusCoinStatus;

    private void Start()
    {
        if (_saveData == null)
        {
            _saveData = new WalletSaveData(this);
            UpdateSaveDataOnContainer();
        }
    }

    public void LoadSaveData(WalletSaveData saveData)
    {
        SetCoin(saveData.GetCoinInWallet);
        _bonusCoinStatus.SetLevel(saveData.GetWalletStatusLevel);

        //_saveData = new WalletSaveData(saveData);
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
        coin = Mathf.Clamp(amount, 0, int.MaxValue);
        OnCoinSetted?.Invoke(coin);
    }

    public void UpdateSaveDataOnContainer()
    {
        _saveData.UpdateData(this);
    }
}
