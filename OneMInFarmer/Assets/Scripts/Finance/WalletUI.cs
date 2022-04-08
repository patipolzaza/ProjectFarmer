using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WalletUI : WindowUIBase
{
    public static WalletUI Instance;
    [SerializeField] private Text CoinText;
    protected override void Awake()
    {
        StartCoroutine(InitialSetUp());
    }

    private IEnumerator InitialSetUp()
    {
        yield return new WaitUntil(() => Player.Instance);
        UpdateCoinText(Player.Instance.wallet.coin);
        Instance = this;
    }

    public void UpdateCoinText(int coin)
    {
        this.CoinText.text = "Coin : " + coin.ToString();
    }


}
