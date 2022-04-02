using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WalletUI : MonoBehaviour
{
    public static WalletUI Instance;
    [SerializeField] private Text CoinText;
    private void Awake()
    {
        StartCoroutine(InitialSetUp());
    }

    private IEnumerator InitialSetUp()
    {
        yield return new WaitUntil(() => Player.Instance);
        SetCoinText(Player.Instance.wallet.coin);
        Instance = this;
    }

    public void SetCoinText(int coin)
    {
        this.CoinText.text = "Coin : " + coin.ToString();
    }
}
