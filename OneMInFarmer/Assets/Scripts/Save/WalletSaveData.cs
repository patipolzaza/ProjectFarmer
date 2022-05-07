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
        _walletStatusLevel = wallet.GetStatus.currentLevel;
        _coinInWallet = wallet.coin;
    }
}
