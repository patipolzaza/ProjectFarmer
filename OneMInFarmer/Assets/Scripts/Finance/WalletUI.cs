using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WalletUI : WindowUIBase
{
    [SerializeField] private Text CoinText;
    private Wallet _playerWallet;
    protected override void Awake()
    {
        StartCoroutine(InitialSetUp());
    }

    private void OnEnable()
    {
        if (_playerWallet == null)
        {
            _playerWallet = FindObjectOfType<Player>().wallet;
        }

        _playerWallet.OnCoinChanged += UpdateCoinTextToTarget;
    }

    private void OnDisable()
    {
        if (_playerWallet == null)
        {
            _playerWallet = FindObjectOfType<Player>().wallet;
        }

        _playerWallet.OnCoinChanged -= UpdateCoinTextToTarget;
    }

    private IEnumerator InitialSetUp()
    {
        yield return new WaitUntil(() => Player.Instance);
        UpdateCoinText(Player.Instance.wallet.coin);
    }

    public void UpdateCoinText(int coin)
    {
        this.CoinText.text = "Coin : " + coin.ToString();
    }

    public void UpdateCoinTextToTarget(int from, int target)
    {
        UpdateCoinText(target);
    }
}
