using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WalletUI : MonoBehaviour
{
    public static WalletUI Instance ;
    [SerializeField] private Text CoinText;
    private void Awake()
    {
        Instance = this;
    }

    public void SetCoinText(int coin)
    {
        this.CoinText.text = "Coin : " + coin.ToString();
    }
}
