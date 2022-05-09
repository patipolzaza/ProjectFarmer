using System;
using UnityEngine;

[System.Serializable]
public class WalletSaveData
{
    [SerializeField] private int _walletStatusLevel;
    [SerializeField] private int _coinInWallet;

    public int GetWalletStatusLevel => _walletStatusLevel;
    public int GetCoinInWallet => _coinInWallet;

    public WalletSaveData(Wallet wallet)
    {
        UpdateData(wallet);
    }

    public void UpdateData(Wallet wallet)
    {
        _walletStatusLevel = wallet.GetStatus.currentLevel;
        _coinInWallet = wallet.coin;

        ObjectDataContainer.UpdateWalletSaveData(this);
    }

    public override string ToString()
    {
        return $"WalletSaveData(extraCoinStatusLevel: {_walletStatusLevel}, coinInWallet: {_coinInWallet})";
    }
}
