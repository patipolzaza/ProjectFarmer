using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WalletUI : WindowUIBase
{
    [SerializeField] private Text CoinText;
    private Wallet _playerWallet;

    private Coroutine _slideTextCoroutine;
    protected override void Awake()
    {
        StartCoroutine(InitialSetUp());
    }

    private void OnEnable()
    {
        if (_playerWallet == null)
        {
            _playerWallet = FindObjectOfType<Player>().transform.Find("Wallet").GetComponent<Wallet>();
        }
        _playerWallet.OnCoinChanged += UpdateCoinTextToTarget;
    }

    private void OnDisable()
    {
        _playerWallet.OnCoinChanged -= UpdateCoinTextToTarget;
    }

    private IEnumerator InitialSetUp()
    {
        yield return new WaitUntil(() => Player.Instance);
        SetCoinText(FindObjectOfType<Wallet>().GetComponent<Wallet>().coin);
    }

    public void SetCoinText(int coin)
    {
        CoinText.text = coin.ToString();
    }

    public void UpdateCoinTextToTarget(int oldValue, int newValue)
    {
        if (_slideTextCoroutine != null)
        {
            StopCoroutine(_slideTextCoroutine);
            _slideTextCoroutine = null;
        }

        StartCoroutine(SlideCoinToTarget(oldValue, newValue));
    }

    private IEnumerator SlideCoinToTarget(float currentValue, float target)
    {
        float slideSpeed = 0.15f;
        do
        {
            currentValue = Mathf.Lerp(currentValue, target, (currentValue / target) * slideSpeed);
            int currentRoundedValue = Mathf.RoundToInt(currentValue);
            SetCoinText(currentRoundedValue);
            yield return new WaitForFixedUpdate();
        } while (Mathf.RoundToInt(currentValue) != target);

        _slideTextCoroutine = null;
    }
}
