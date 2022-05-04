using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DebtManager : MonoBehaviour
{
    public static DebtManager Instance { get; private set; }
    public int dayForNextDebtPayment { get; private set; } = 5;
    public int debtPaidCount { get; private set; }
    private float _debtMultiplierPerPeriod = 1.4f;
    private int _startDebt = 10;
    public int remainingDebt { get; private set; } = 0;

    private Coroutine _delayPayDebtCoroutine = null;

    public UnityEvent OnShowedResultUI;
    public UnityEvent OnHidedResultUI;
    public UnityEvent OnProcessSkipped;
    public UnityEvent OnDebtPaid;

    private bool _isUIShowed = false;
    private bool _isDebtPaid = false;
    private bool _canSkip = false;
    public bool isAllProcessFinished { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        remainingDebt = GetDebt;
    }

    private void Update()
    {
        if (!_isUIShowed)
        {
            return;
        }

        if (Input.anyKeyDown && _isUIShowed && _canSkip)
        {
            SkipProcess();
        }

        _canSkip = true;
    }

    public int GetDebt
    {
        get
        {
            float debt = _startDebt + (debtPaidCount) * _debtMultiplierPerPeriod;

            return Mathf.RoundToInt(debt);
        }
    }

    public int GetDayRemainForDebtPayment
    {
        get
        {
            return dayForNextDebtPayment - GameManager.Instance.currentDay;
        }
    }

    public void ShowResultUI()
    {
        _isUIShowed = true;
        OnShowedResultUI?.Invoke();
    }

    public void HideResultUI()
    {
        _isUIShowed = false;
        OnHidedResultUI?.Invoke();
    }

    ///<summary>
    ///Pay the debt and after that will increase paid count that affect to next debt payment
    ///</summary>
    public void PayDebt()
    {
        if (_isDebtPaid)
        {
            return;
        }
        Wallet playerWallet = Player.Instance.wallet;
        int playerCoin = playerWallet.coin;
        int debt = remainingDebt;

        if (playerCoin < debt)
        {
            remainingDebt -= playerCoin;
            playerWallet.LoseCoin(playerCoin);
        }
        else
        {
            playerWallet.LoseCoin(debt);
            remainingDebt = 0;

            int score = debt * debtPaidCount;

            dayForNextDebtPayment += 5;
            debtPaidCount++;

            ScoreManager.Instance.AddScore(score);
        }

        _isDebtPaid = true;
        isAllProcessFinished = true;

        OnDebtPaid?.Invoke();
    }

    private void SkipProcess()
    {
        if (_delayPayDebtCoroutine != null)
        {
            StopCoroutine(_delayPayDebtCoroutine);
        }

        OnProcessSkipped?.Invoke();
    }

    public void ResetParameters()
    {
        _isDebtPaid = false;
        isAllProcessFinished = false;
        _canSkip = false;
    }

    public void PayDebtWithDelayTime(float delayTime)
    {
        _delayPayDebtCoroutine = StartCoroutine(DelayedPayDebt(delayTime));
    }

    private IEnumerator DelayedPayDebt(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        PayDebt();
        _delayPayDebtCoroutine = null;
    }
}
