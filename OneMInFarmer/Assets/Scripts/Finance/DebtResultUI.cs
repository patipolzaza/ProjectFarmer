using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebtResultUI : WindowUIBase
{
    [SerializeField] private Text _debtCostText;
    [SerializeField] private Text _playerCoinText;
    [SerializeField] private GameObject _continueTextObject;

    private bool _canContinue = false;

    private void OnEnable()
    {
        Player.Instance.wallet.OnCoinChanged += UpdatePlayerCoin;
    }
    private void OnDisable()
    {
        Player.Instance.wallet.OnCoinChanged -= UpdatePlayerCoin;

        HideContinueText();
    }

    protected override void Update()
    {
        if (!DebtManager.Instance || !DebtManager.Instance.isAllProcessFinished || !_continueTextObject.activeSelf)
        {
            return;
        }

        if (Input.anyKeyDown && _canContinue)
        {
            if (DebtManager.Instance.remainingDebt == 0)
            {
                GameManager.Instance.ToNextDay();
                DebtManager.Instance.HideResultUI();
                _canContinue = false;
            }
            else
            {
                GameManager.Instance.GameOver();
            }
        }

        _canContinue = true;
    }

    public void SetUpUI()
    {
        SetDebtCostText(DebtManager.Instance.GetDebt.ToString());
        SetPlayerCoinText(Player.Instance.wallet.coin.ToString());
    }

    public void SetDebtCostText(string newText)
    {
        _debtCostText.text = $"{newText}";
    }

    public void UpdateDeptCostText()
    {
        float currentValue = float.Parse(_debtCostText.text);
        int targetValue = DebtManager.Instance.remainingDebt;
        StartCoroutine(SlideDebtCostToTarget(currentValue, targetValue));
        return;
    }

    private IEnumerator SlideDebtCostToTarget(float currentValue, float target)
    {
        float slideSpeed = 0.15f;
        do
        {
            currentValue = Mathf.Lerp(currentValue, target, slideSpeed);
            string currentValueText = Mathf.RoundToInt(currentValue).ToString();
            SetDebtCostText(currentValueText);
            yield return new WaitForSeconds(0.01f);

        } while (Mathf.RoundToInt(currentValue) != target);

        ShowContinueText();
        yield return null;
    }

    public void SetPlayerCoinText(string newText)
    {
        _playerCoinText.text = newText;
    }

    public void UpdatePlayerCoin(int oldValue, int newValue)
    {
        StartCoroutine(SlidePlayerCoinTextToTarget(oldValue, newValue));
    }

    private IEnumerator SlidePlayerCoinTextToTarget(float currentValue, float target)
    {
        float slideSpeed = 0.15f;
        float maxDistance = target > currentValue ? (target - currentValue) : (currentValue - target);
        float distanceFromStart = 0;
        do
        {
            currentValue = Mathf.Lerp(currentValue, target, Mathf.Clamp((distanceFromStart / maxDistance) * slideSpeed, 0.2f, 1.5f));
            string currentValueText = Mathf.RoundToInt(currentValue).ToString();
            SetPlayerCoinText(currentValueText);
            yield return new WaitForFixedUpdate();
        } while (Mathf.RoundToInt(currentValue) != target);

        ShowContinueText();
    }

    public void ShowSkippedResult()
    {
        DebtManager.Instance.PayDebt();
        StopAllCoroutines();
        SetPlayerCoinText(Player.Instance.wallet.coin.ToString());
        SetDebtCostText(DebtManager.Instance.remainingDebt.ToString());

        ShowContinueText();
    }

    public void ShowContinueText()
    {
        _continueTextObject.SetActive(true);
    }

    public void ResetParameters()
    {
        _canContinue = false;
    }

    public void HideContinueText()
    {
        _continueTextObject.SetActive(false);
    }

    public void StopSlideText()
    {
        StopAllCoroutines();
    }
}
